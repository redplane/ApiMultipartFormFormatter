using ApiMultiPartFormData.Models;

namespace ApiBackEnd.ViewModels
{
    public class HttpFileBaseResponseViewModel
    {
        #region Properties

        /// <summary>
        ///     Name of file.
        /// </summary>
        public string FileName { get; set; }

        public string ContentType { get; set; }

        public long ContentLength { get; set; }

        #endregion

        #region Constructor

        public HttpFileBaseResponseViewModel()
        {
        }

        public HttpFileBaseResponseViewModel(HttpFileBase model)
        {
            FileName = model.FileName;
            ContentType = model.ContentType;
            ContentLength = model.ContentLength;
        }

        #endregion
    }
}