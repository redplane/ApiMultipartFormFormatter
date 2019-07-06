using System;

namespace ApiMultiPartFormData
{
    [Obsolete("Please use MultipartFormDataFormatter class instead. This class will be removed in next version.")]
    public class ApiMultipartFormDataFormatter : MultipartFormDataFormatter
    {
        #region Constructor

        public ApiMultipartFormDataFormatter()
        {
            throw new Exception($"{nameof(ApiMultipartFormDataFormatter)} is obsoleted. Please use {nameof(MultipartFormDataFormatter)} instead.");
        }

        #endregion
    }
}