#if NETFRAMEWORK
using System.Net.Http;
#elif NETCOREAPP
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
#endif


namespace ApiMultiPartFormData.Extensions
{
    public static class HttpContentExtensions
    {
#if NETFRAMEWORK
        public static bool IsAttachment(this HttpContent httpContent)
        {
            var fileName = httpContent.Headers.ContentDisposition.FileName;
            if (string.IsNullOrWhiteSpace(fileName))
                return false;

            return httpContent.Headers.ContentLength.GetValueOrDefault() > 0;
        }

        public static string GetContentDispositionName(this HttpContent httpContent)
        {
            return httpContent.Headers.ContentDisposition.Name.Trim('"');
        }

        public static string GetFileName(this HttpContent httpContent)
        {
            return httpContent.Headers
                .ContentDisposition
                .FileName
                .Trim('"');
        }

        public static string GetContentType(this HttpContent httpContent)
        {
            return httpContent.Headers
                .ContentType
                .MediaType;
        }
#endif

#if NETCOREAPP
        public static bool IsAttachment(this KeyValuePair<string, StringValues> httpContent)
        {
            return false;
        }

        public static Task<string> ReadAsStringAsync(this KeyValuePair<string, StringValues> httpContent)
        {
            var value = httpContent.Value.ToString();
            return Task.FromResult(value);
        }

        public static string GetContentDispositionName(this KeyValuePair<string, StringValues> httpContent)
        {
            return httpContent.Key.Trim();
        }

        public static string GetContentDispositionName(this IFormFile attachment)
        {
            return attachment.Name.Trim();
        }

        public static string GetFileName(this IFormFile attachment)
        {
            return attachment.FileName?.Trim();
        }

        public static Task<Stream> ReadAsStreamAsync(this IFormFile attachment)
        {
            var stream = attachment.OpenReadStream();
            return Task.FromResult(stream);
        }

        public static string GetContentType(this IFormFile attachment)
        {
            return attachment.ContentType;
        }

        public static bool IsAttachment(this IFormFile attachment)
        {
            return true;
        }
#endif
    }
}