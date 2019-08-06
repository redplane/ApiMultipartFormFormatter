namespace ApiMultiPartFormData.Models
{
    public class HttpFile : HttpFileBase
    {
        #region Properties

        /// <summary>
        ///     Serialized byte stream of file.
        /// </summary>
        public byte[] Buffer { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        ///     Initialize an instance of HttpFile without any setting.
        /// </summary>
        public HttpFile()
        {
        }

        /// <summary>
        ///     Initialize an instance of HttpFile with parameters.
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="mediaType"></param>
        /// <param name="buffer"></param>
        public HttpFile(string fileName, string mediaType, byte[] buffer)
        {
            Name = fileName;
            MediaType = mediaType;
            Buffer = buffer;
        }

        #endregion
    }
}