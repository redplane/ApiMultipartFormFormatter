#if NETFRAMEWORK
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using ApiMultiPartFormData.Extensions;
using ApiMultiPartFormData.Models;
using System;
using System.Net.Http.Formatting;

namespace ApiMultiPartFormData
{

    public partial class MultipartFormDataFormatter : MediaTypeFormatter
    {
        /// <summary>
        ///     Whether the instance can be read or not.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public override bool CanReadType(Type type)
        {
            if (type == null) 
                throw new ArgumentNullException(nameof(type));
            return true;
        }

        /// <summary>
        ///     Whether the instance can be written or not.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public override bool CanWriteType(Type type)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));
            return false;
        }

        /// <summary>
        ///     Read data from incoming stream.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="stream"></param>
        /// <param name="content"></param>
        /// <param name="logger"></param>
        /// <returns></returns>
        public override async Task<object> ReadFromStreamAsync(Type type, Stream stream, HttpContent content,
            IFormatterLogger logger)
        {

            // Stream is invalid.
            if (stream == null)
                throw new ArgumentNullException(nameof(stream));

            // Type is invalid.
            if (type == null)
                throw new ArgumentNullException(nameof(type));

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
                    var contentParameter = httpContent.GetContentDispositionName();
                    var parameterParts = FindContentDispositionParameters(contentParameter);

                    // Content is a parameter, not a file.
                    if (!httpContent.IsAttachment())
                    {
                        var value = await httpContent.ReadAsStringAsync();
                        await BuildRequestModelAsync(instance, parameterParts, value);
                        continue;
                    }

                    // Content is a file.
                    // File retrieved from client-side.
                    HttpFileBase file = null;

                    // set null if no content was submitted to have support for [Required]
                    if (httpContent.Headers.ContentLength.GetValueOrDefault() > 0)
                    {
                        file = new HttpFileBase(
                            httpContent.GetFileName(),
                            await httpContent.ReadAsStreamAsync(),
                            httpContent.GetContentType());
                    }

                    await BuildRequestModelAsync(instance, parameterParts, file);
                }

                return instance;
            }
            catch (Exception e)
            {
                // TODO: Implement logger.
                //if (logger == null)
                //    throw;
                //logger.LogError(string.Empty, e);
                var defaultValue = GetDefaultValueForType(type);
                return defaultValue;
            }

        }
    }
}

#endif