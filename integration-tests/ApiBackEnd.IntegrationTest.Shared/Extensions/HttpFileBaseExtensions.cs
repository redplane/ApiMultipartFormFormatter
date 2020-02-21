using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using ApiMultiPartFormData.Models;

namespace ApiBackEnd.IntegrationTest.Shared.Extensions
{
    public static class HttpFileBaseExtensions
    {
#if NETFRAMEWORK
        public static ByteArrayContent ToByteArrayContent(this HttpFileBase attachment, string parameter)
        {
            var memoryStream = new MemoryStream();
            attachment.InputStream.CopyTo(memoryStream);
            var content = new ByteArrayContent(memoryStream.ToArray());
            content.Headers.ContentType = MediaTypeHeaderValue.Parse(attachment.ContentType);
            content.Headers.ContentDisposition = ContentDispositionHeaderValue.Parse("attachment");
            content.Headers.ContentDisposition.FileName = attachment.FileName;
            content.Headers.ContentDisposition.Name = parameter;

            return content;
        }

        public static ByteArrayContent ToByteArrayContent(this HttpFile attachment, string parameter)
        {
            var content = new ByteArrayContent(attachment.Buffer);
            content.Headers.ContentType = MediaTypeHeaderValue.Parse(attachment.ContentType);
            content.Headers.ContentDisposition = ContentDispositionHeaderValue.Parse("attachment");
            content.Headers.ContentDisposition.FileName = attachment.FileName;
            content.Headers.ContentDisposition.Name = parameter;

            return content;
        }
#elif NETSTANDARD
        public static byte[] ToBytes(this HttpFileBase attachment)
        {
            using (var memoryStream = new MemoryStream())
            {
                attachment.InputStream.CopyTo(memoryStream);
                return memoryStream.ToArray();
            }
        }

        public static byte[] ToBytes(this HttpFile attachment)
        {
            return ((HttpFileBase)attachment).ToBytes();
        }
#endif
    }
}
