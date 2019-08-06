using System;
using System.IO;

namespace ApiMultiPartFormData.Models
{
    public class StreamedHttpFile : HttpFileBase
    {
        #region Properties

        /// <summary>
        ///     Stream containing file contents.
        /// </summary>
        public Stream Stream { get; set; }

        /// <summary>
        ///     Length of file stream
        /// </summary>
        public long StreamLength
        {
            get
            {
                return Stream.Length;
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        ///     Initialize an instance of StreamedHttpFile without any setting.
        /// </summary>
        public StreamedHttpFile()
        {
        }

        /// <summary>
        ///     Initialize an instance of StreamedHttpFile with parameters.
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="mediaType"></param>
        /// <param name="stream"></param>
        public StreamedHttpFile(string fileName, string mediaType, Stream stream)
        {
            Name = fileName;
            MediaType = mediaType;
            Stream = stream;
        }

        #endregion
    }
}