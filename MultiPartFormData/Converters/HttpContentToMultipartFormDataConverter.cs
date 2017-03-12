using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using MultiPartFormData.Converters.Original;
using MultiPartFormData.Models;

namespace MultiPartFormData.Converters
{
    public class HttpContentToMultipartFormDataConverter : HttpContentOriginalConverter
    {
        private readonly Regex _contentHeaderParser;

        public HttpContentToMultipartFormDataConverter()
        {
            _contentHeaderParser = new Regex("(?:([\\w\\s]+)\\|?)");
        }
        /// <summary>
        ///     This function is for converting HttpContent instance to FormFile instance.
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public async Task<MultipartFormData> Convert(HttpContent content)
        {
            // Invalid content.
            if (content == null)
                return null;

            // Content is unsupported.
            if (!content.IsMimeMultipartContent())
                throw new Exception("Unsupported Media Type");

            // Read stream provider from content.
            var multipartMemoryStreamProvider = await content.ReadAsMultipartAsync();

            // Retrieve the multipart form data from provider.
            var formFile = await Convert(multipartMemoryStreamProvider);
            return formFile;
        }

        /// <summary>
        ///     Convert MultipartMemoryStreamProvider instance to FormFile.
        /// </summary>
        /// <param name="multipartProvider"></param>
        /// <returns></returns>
        public async Task<MultipartFormData> Convert(MultipartMemoryStreamProvider multipartProvider)
        {
            // Initialize an instance from form file.
            var formFile = new MultipartFormData();

            // Loop through every content to put file into FormFile instance.
            foreach (var file in multipartProvider.Contents.Where(x => IsFile(x.Headers.ContentDisposition)))
            {
                var name = RemoveQuotes(file.Headers.ContentDisposition.Name);
                var fileName = FixFilename(file.Headers.ContentDisposition.FileName);
                var mediaType = file.Headers.ContentType.MediaType;

                using (var stream = await file.ReadAsStreamAsync())
                {
                    var buffer = ReadAllBytes(stream);
                    if (buffer.Length > 0)
                        formFile.Add(name, new HttpFile(fileName, mediaType, buffer));
                }
            }

            var parameters = new Dictionary<string, KeyValuePair<string, object>>();
            var pairs = new List<KeyValuePair<string, string>>();
            // Loop through every content to put data into FormFile instance.
            foreach (
                var part in
                multipartProvider.Contents.Where(x => (x.Headers.ContentDisposition.DispositionType == "form-data")
                                                      && !IsFile(x.Headers.ContentDisposition)))
            {
                var dictionary = new Dictionary<string, object>();
                var pair = new KeyValuePair<string, object>();
                var name = RemoveQuotes(part.Headers.ContentDisposition.Name);
                var h = name.Replace("[", ",").Replace("]", ",").Split(',').Where(x => !string.IsNullOrEmpty(x));

                foreach (var p in h)
                {
                }

                var data = await part.ReadAsStringAsync();
                pairs.Add(new KeyValuePair<string, string>(name, data));
                formFile.Add(name, data);
            }
            
            return formFile;
        }

        //private async Task FindParameterAsync(HttpContent httpContent)
        //{
        //}
    }
}