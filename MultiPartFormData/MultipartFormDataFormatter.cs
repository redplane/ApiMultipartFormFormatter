using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
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

        public override bool CanReadType(Type type)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));
            return true;
        }

        public override bool CanWriteType(Type type)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));
            return false;
        }

        public override async Task<object> ReadFromStreamAsync(Type type, Stream readStream, HttpContent content,
            IFormatterLogger formatterLogger)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));
            if (readStream == null) throw new ArgumentNullException(nameof(readStream));

            try
            {
                // load multipart data into memory 
                var multipartProvider = await content.ReadAsMultipartAsync();
                var httpContents = multipartProvider.Contents;

                IDictionary<string, object> parameters = new Dictionary<string, object>();
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
                        Read(parameters, parameterParts, value);
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

                    Read(parameters, parameterParts, file);
                }

                var member = type.GetMember("Owner").GetType();
                //var t = owner.GetType();
                var a = Activator.CreateInstance(type);
                var k = type.GetProperty("Owner");
                return null;
            }
            catch (Exception e)
            {
                if (formatterLogger == null)
                    throw;
                formatterLogger.LogError(string.Empty, e);
                return GetDefaultValueForType(type);
            }
        }

        private void Read(IDictionary<string, object> model, IList<string> parameters, object value)
        {
            object pointer = model;
            var lastKey = parameters[parameters.Count - 1];
            for (var index = 0; index < parameters.Count - 1; index++)
            {
                var dictionary = (Dictionary<string, object>) pointer;
                var key = parameters[index];
                if (dictionary.ContainsKey(key))
                {
                    pointer = dictionary[key];
                    continue;
                }

                //var pair = new Dictionary<string, object>();
                var val = new Dictionary<string, object>();
                dictionary.Add(key, val);
                //((Dictionary<string, object>)pointer).Add(key, pair);
                pointer = val;
            }

            ((Dictionary<string, object>) pointer)[lastKey] = value;
        }
    }
}