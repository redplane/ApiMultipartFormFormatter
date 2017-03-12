using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using MultiPartFormData.Infrastructure.Extensions;
using MultiPartFormData.Models;

namespace MultiPartFormData.Converters.Original
{
    public class MultipartFormDataAnalyzer
    {
        protected byte[] GetMultipartFormDataBytes(IEnumerable<KeyValuePair<string, object>> postParameters,
            string boundary)
        {
            var encoding = Encoding.UTF8;

            using (var formDataStream = new MemoryStream())
            {
                var needsClrf = false;

                foreach (var param in postParameters)
                {
                    // Add a CRLF to allow multiple parameters to be added.
                    // Skip it on the first parameter, add it to subsequent parameters.
                    if (needsClrf)
                        formDataStream.Write(encoding.GetBytes("\r\n"), 0, encoding.GetByteCount("\r\n"));

                    needsClrf = true;

                    if (param.Value is HttpFile)
                    {
                        var httpFileToUpload = (HttpFile) param.Value;

                        // Add just the first part of this param, since we will write the file data directly to the Stream
                        var header =
                            $"--{boundary}\r\nContent-Disposition: form-data; name=\"{param.Key}\"; filename=\"{httpFileToUpload.Name ?? param.Key}\"\r\nContent-Type: {httpFileToUpload.MediaType ?? "application/octet-stream"}\r\n\r\n";

                        formDataStream.Write(encoding.GetBytes(header), 0, encoding.GetByteCount(header));

                        // Write the file data directly to the Stream, rather than serializing it to a string.
                        formDataStream.Write(httpFileToUpload.Buffer, 0, httpFileToUpload.Buffer.Length);

                        continue;
                    }

                    var objString = "";
                    if (param.Value != null)
                    {
                        var typeConverter = param.Value.GetType().GetToStringConverter();
                        if (typeConverter != null)
                            objString = typeConverter.ConvertToString(null, CultureInfo.CurrentCulture, param.Value);
                        else
                            throw new Exception(string.Format("Type \"{0}\" cannot be converted to string",
                                param.Value.GetType().FullName));
                    }

                    var postData =
                        $"--{boundary}\r\nContent-Disposition: form-data; name=\"{param.Key}\"\r\n\r\n{objString}";
                    formDataStream.Write(encoding.GetBytes(postData), 0, encoding.GetByteCount(postData));
                }

                // Add the end of the request.  Start with a newline
                var footer = "\r\n--" + boundary + "--\r\n";
                formDataStream.Write(encoding.GetBytes(footer), 0, encoding.GetByteCount(footer));

                var formData = formDataStream.ToArray();

                return formData;
            }
        }

        protected void FillFlatPropertiesListFromObject(object obj, string prefix,
            ICollection<KeyValuePair<string, object>> propertiesList)
        {
            if (obj == null) return;
            var type = obj.GetType();

            var dictionary = obj as IDictionary;
            if (dictionary != null)
            {
                var dict = dictionary;
                var index = 0;
                foreach (var key in dict.Keys)
                {
                    var indexedKeyPropName = $"{prefix}[{index}].Key";
                    FillFlatPropertiesListFromObject(key, indexedKeyPropName, propertiesList);

                    var indexedValuePropName = $"{prefix}[{index}].Value";
                    FillFlatPropertiesListFromObject(dict[key], indexedValuePropName, propertiesList);

                    index++;
                }
            }
            else if (obj is ICollection)
            {
                var list = obj as ICollection;
                var index = 0;
                foreach (var indexedPropValue in list)
                {
                    var indexedPropName = $"{prefix}[{index}]";
                    FillFlatPropertiesListFromObject(indexedPropValue, indexedPropName, propertiesList);

                    index++;
                }
            }
            else if (type.IsCustomNonEnumerableType())
            {
                foreach (var propertyInfo in type.GetPublicAccessibleProperties())
                {
                    var propName = string.IsNullOrWhiteSpace(prefix)
                        ? propertyInfo.Name
                        : $"{prefix}.{propertyInfo.Name}";
                    var propValue = propertyInfo.GetValue(obj);

                    FillFlatPropertiesListFromObject(propValue, propName, propertiesList);
                }
            }
            else
            {
                propertiesList.Add(new KeyValuePair<string, object>(prefix, obj));
            }
        }

        /// <summary>
        ///     Fill the properties which are retrieved from FormFile instance.
        /// </summary>
        /// <param name="formFile"></param>
        /// <param name="propertiesList"></param>
        protected void FillFlatPropertiesListFromFormData(MultipartFormData formFile,
            List<KeyValuePair<string, object>> propertiesList)
        {
            propertiesList.AddRange(
                formFile.Fields.Select(field => new KeyValuePair<string, object>(field.Key, field.Value)));
            propertiesList.AddRange(
                formFile.Files.Select(field => new KeyValuePair<string, object>(field.Key, field.Value)));
        }
    }
}