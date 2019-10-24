namespace ApiMultiPartFormData.Models
{
    public abstract class HttpFileBase
    {
        #region Properties

        /// <summary>
        ///     Name of file.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///     Type of file.
        /// </summary>
        public string MediaType { get; set; }

        #endregion
    }
}