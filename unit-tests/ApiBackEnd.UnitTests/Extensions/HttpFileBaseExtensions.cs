using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using ApiMultiPartFormData.Models;

namespace ApiBackEnd.UnitTests.Extensions
{
    public static class HttpFileBaseExtensions
    {
        public static ByteArrayContent ToByteArrayContent(this HttpFileBase attachment)
        {
            var memoryStream = new MemoryStream();
            attachment.InputStream.CopyTo(memoryStream);
            var content = new ByteArrayContent(memoryStream.ToArray());
            content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/octet-stream");
            content.Headers.ContentDisposition = ContentDispositionHeaderValue.Parse(attachment.FileName);

            return content;
        }

        public static ByteArrayContent ToByteArrayContent(this HttpFile attachment)
        {
            var content = new ByteArrayContent(attachment.Buffer);
            content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/octet-stream");
            content.Headers.ContentDisposition = ContentDispositionHeaderValue.Parse(attachment.FileName);

            return content;
        }
    }
}
