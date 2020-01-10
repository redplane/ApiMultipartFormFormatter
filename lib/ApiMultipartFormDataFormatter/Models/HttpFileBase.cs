using System.IO;

namespace ApiMultiPartFormData.Models
{
    public class HttpFileBase
    {
        #region Properties

        /// <summary>
        ///     Name of file.
        /// </summary>
        public string FileName { get; }

        public Stream InputStream { get; }

        public string ContentType { get; }

        public long ContentLength { get; }

        #endregion

        #region Constructor

        internal HttpFileBase()
        {

        }

        public HttpFileBase(string fileName, Stream inputStream, string contentType)
        {
            FileName = fileName;
            InputStream = inputStream;
            ContentType = contentType;
            ContentLength = inputStream.Length;
        }

        #endregion
    }
}