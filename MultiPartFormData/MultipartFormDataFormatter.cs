using System;
using System.Collections.Generic;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ApiMultiPartFormData.Models;

namespace MultiPartFormData
{
    public class MultipartFormDataFormatter : MediaTypeFormatter
    {
        private const string SupportedMediaType = "multipart/form-data";

        /// <summary>
        ///     Initializes a new instance of the <see cref="MultipartFormDataFormatter" /> class.
        /// </summary>
        public MultipartFormDataFormatter()
        {
            SupportedMediaTypes.Add(new MediaTypeHeaderValue(SupportedMediaType));
        }

        /// <summary>
        /// Whether the instance can be read or not.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public override bool CanReadType(Type type)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));
            return true;
        }

        /// <summary>
        /// Whether the instance can be written or not.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public override bool CanWriteType(Type type)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));
            return false;
        }

        /// <summary>
        /// Read data from incoming stream.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="stream"></param>
        /// <param name="content"></param>
        /// <param name="formatterLogger"></param>
        /// <returns></returns>
        public override async Task<object> ReadFromStreamAsync(Type type, Stream stream, HttpContent content,
            IFormatterLogger formatterLogger)
        {
            // Type is invalid.
            if (type == null)
                throw new ArgumentNullException(nameof(type));

            // Stream is invalid.
            if (stream == null)
                throw new ArgumentNullException(nameof(stream));


            try
            {
                // load multipart data into memory 
                var multipartProvider = await content.ReadAsMultipartAsync();
                var httpContents = multipartProvider.Contents;

                // Create an instance from specific type.
                var instance = Activator.CreateInstance(type);

                foreach (var httpContent in httpContents)
                {
                    // Find parameter from content deposition.
                    var contentParameter = httpContent.Headers.ContentDisposition.Name.Trim('"');
                    var parameterParts =
                        contentParameter.Replace("[", ",")
                            .Replace("]", ",")
                            .Split(',')
                            .Where(x => !string.IsNullOrEmpty(x))
                            .ToList();

                    // Content is a parameter, not a file.
                    if (string.IsNullOrEmpty(httpContent.Headers.ContentDisposition.FileName))
                    {
                        var value = await httpContent.ReadAsStringAsync();
                        Read(instance, parameterParts, value);
                        continue;
                    }

                    // Content is a file.
                    // File retrieved from client-side.
                    HttpFile file;

                    // set null if no content was submitted to have support for [Required]
                    if (httpContent.Headers.ContentLength.GetValueOrDefault() > 0)
                        file = new HttpFile(
                            httpContent.Headers.ContentDisposition.FileName.Trim('"'),
                            httpContent.Headers.ContentType.MediaType,
                            await httpContent.ReadAsByteArrayAsync()
                        );
                    else
                        file = null;

                    Read(instance, parameterParts, file);
                }

                return instance;
            }
            catch (Exception e)
            {
                if (formatterLogger == null)
                    throw;
                formatterLogger.LogError(string.Empty, e);
                return GetDefaultValueForType(type);
            }
        }

        /// <summary>
        /// Read parameters list and bind information to model.
        /// </summary>
        /// <param name="model"></param>
        /// <param name="parameters"></param>
        /// <param name="value"></param>
        private void Read(object model, IList<string> parameters, object value)
        {
            // Initiate model pointer.
            var pointer = model;

            // Find the last key.
            //var lastKey = parameters[parameters.Count - 1];

            // Initiate property information.
            PropertyInfo propertyInfo = null;

            if (parameters == null || parameters.Count < 1)
                return;

            for (var index = 0; index < parameters.Count; index++)
            {
                // Find parameter key.
                var key = parameters[index];
                
                // Numeric key is always about array.
                if (IsNumeric(key))
                {
                    // Invalid property info.
                    if (propertyInfo == null)
                        return;

                    // Current property information is not a list.
                    if (!IsList(propertyInfo.PropertyType))
                        return;

                    // Find the index of parameter.
                    var iCollectionIndex = 0;
                    if (!int.TryParse(key, out iCollectionIndex))
                        return;

                    // Add new property into list.
                    object val = null;

                    // This is the last key.
                    if (index + 1 >= parameters.Count)
                    {
                        InitiateArrayMember(pointer, iCollectionIndex, propertyInfo, value);
                        return;
                    }

                    val = InitiateArrayMember(pointer, iCollectionIndex, propertyInfo);
                    pointer = val;

                    // Find the property information of the next key.
                    var nextKey = parameters[index + 1];
                    propertyInfo = FindInstanceProperty(pointer, nextKey);
                    continue;
                }

                // Find property of the current key.
                propertyInfo = FindInstanceProperty(pointer, key);

                // Property doesn't exist.
                if (propertyInfo == null)
                    return;

                // This is the last parameter.
                if (index + 1 >= parameters.Count)
                {
                    propertyInfo.SetValue(pointer, Convert.ChangeType(value, propertyInfo.PropertyType));
                    return;
                }

                // Find property value.
                var pVal = propertyInfo.GetValue(pointer);

                // Value doesn't exist.
                if (pVal == null)
                {
                    // Initiate property value.
                    pVal = Convert.ChangeType(Activator.CreateInstance(propertyInfo.PropertyType),
                        propertyInfo.PropertyType);
                    propertyInfo.SetValue(pointer, pVal);
                    pointer = pVal;
                }
                else
                {
                    // Value is list.
                    if (IsList(propertyInfo.PropertyType))
                    {
                        pointer = propertyInfo.GetValue(pointer);
                        InitiateArrayMember(pointer, -1, propertyInfo, value);
                        continue;
                    }

                    // Go to next key
                    pointer = pVal;
                }
            }
        }

        /// <summary>
        /// Add or update member of array.
        /// </summary>
        /// <param name="pointer"></param>
        /// <param name="iCollectionIndex"></param>
        /// <param name="propertyInfo"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        private object InitiateArrayMember(object pointer, int iCollectionIndex, PropertyInfo propertyInfo, object value = null)
        {
            var itemCount = (int)propertyInfo.PropertyType.GetProperty("Count").GetValue(pointer, null);
            var genericArguments = propertyInfo.PropertyType.GetGenericArguments();

            //var iGenericListArgumentTotal = propertyInfo.PropertyType.GetMethod("Count").Invoke(pointer, new[] { null });
            if (iCollectionIndex < 0 || iCollectionIndex > itemCount - 1)
            {
                object listItem = null;
                if (value != null)
                    listItem = value;
                else
                    listItem = Activator.CreateInstance(propertyInfo.PropertyType.GetGenericArguments()[0]);
                propertyInfo.PropertyType.GetMethod("Add").Invoke(pointer, new[] { listItem });
                return listItem;
            }

            var commandSelectItem = typeof(Enumerable)
                .GetMethod("ElementAt")
                .MakeGenericMethod(genericArguments[0]);

            // Find item at specific index.
            return commandSelectItem.Invoke(pointer, new[] { pointer, iCollectionIndex });
        }
        

        /// <summary>
        /// Find property information of an instance by using property name.
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        private PropertyInfo FindInstanceProperty(object instance, string name)
        {
            return
                    instance.GetType()
                        .GetProperties()
                        .FirstOrDefault(x => name.Equals(x.Name, StringComparison.InvariantCultureIgnoreCase));
        }

        /// <summary>
        /// Check whether text is only numeric or not.
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        private bool IsNumeric(string text)
        {
            var regexNumeric = new Regex("^[0-9]*$");
            return regexNumeric.IsMatch(text);
        }

        /// <summary>
        /// Whether instance is a collection or not.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private bool IsList(Type type)
        {
            return type.IsGenericType && type.GetGenericTypeDefinition()
                    == typeof(List<>);
        }
    }
}