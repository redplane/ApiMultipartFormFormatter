using System;
using System.IO;

namespace ApiMultiPartFormData.Models
{
    public class HttpFile : HttpFileBase
    {
        #region Accessors

        /// <summary>
        ///     Name of file.
        /// </summary>
        public string Name => FileName;

        /// <summary>
        ///     Type of file.
        /// </summary>
        public string MediaType => ContentType;

        /// <summary>
        ///     Serialized byte stream of file.
        /// </summary>
        public byte[] Buffer
        {
            get
            {
                if (InputStream == null)
                    return null;

                if (!(InputStream is MemoryStream memoryStream))
                    return null;

                return memoryStream.ToArray();
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        ///     Initialize an instance of HttpFile with parameters.
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="mediaType"></param>
        /// <param name="buffer"></param>
        public HttpFile(string fileName, string mediaType, byte[] buffer) : base(fileName, new MemoryStream(buffer), mediaType)
        {
            // Seek the stream to the beginning.
            if (InputStream.CanSeek)
                InputStream.Seek(0, SeekOrigin.Begin);
        }

        public HttpFile(HttpFileBase httpFileBase) : base(httpFileBase.FileName, ToMemoryStream(httpFileBase.InputStream), httpFileBase.ContentType)
        {
        }

        #endregion

        #region Methods

        protected static MemoryStream ToMemoryStream(Stream stream)
        {
            var memoryStream = new MemoryStream();
            stream.CopyTo(memoryStream);
            memoryStream.Seek(0, SeekOrigin.Begin);
            return memoryStream;
        }

        #endregion
    }
}