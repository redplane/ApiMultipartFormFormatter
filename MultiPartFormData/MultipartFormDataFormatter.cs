using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Reflection;
using System.Threading.Tasks;
using MultiPartFormData.Models;

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
        /// Read and construct nested properties instance base on parameters with final value.
        /// </summary>
        /// <param name="model"></param>
        /// <param name="parameters"></param>
        /// <param name="value"></param>
        private void Read(object model, IList<string> parameters, object value)
        {
            // Initiate model pointer.
            var pointer = model;

            // Find the last key.
            var lastKey = parameters[parameters.Count - 1];

            // Initiate property information.
            PropertyInfo propertyInfo;

            // Loop through every keys.
            for (var index = 0; index < parameters.Count - 1; index++)
            {
                // Find key.
                var key = parameters[index];

                propertyInfo =
                    pointer.GetType()
                        .GetProperties()
                        .FirstOrDefault(x => key.Equals(x.Name, StringComparison.InvariantCultureIgnoreCase));

                // Property hasn't been initialized.
                if (propertyInfo == null)
                    break;

                // Initiate property.
                propertyInfo = pointer.GetType()
                    .GetProperties()
                    .FirstOrDefault(x => key.Equals(x.Name, StringComparison.InvariantCultureIgnoreCase));

                // Property doesn't exist in object.
                if (propertyInfo == null)
                    return;

                var val = propertyInfo.GetValue(pointer);
                if (val == null)
                {
                    val = Convert.ChangeType(Activator.CreateInstance(propertyInfo.PropertyType), propertyInfo.PropertyType);
                    propertyInfo.SetValue(pointer, val);
                    pointer = val;
                    continue;
                }

                pointer = val;
            }

            propertyInfo = pointer.GetType()
                .GetProperties()
                .FirstOrDefault(x => lastKey.Equals(x.Name, StringComparison.InvariantCultureIgnoreCase));

            if (propertyInfo != null)
                propertyInfo.SetValue(pointer, Convert.ChangeType(value, propertyInfo.PropertyType));
        }
    }
}