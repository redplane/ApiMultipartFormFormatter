using System;
using System.ComponentModel.DataAnnotations;
using MultiPartFormData.Models;

namespace MultiPartFormData.Attributes
{
    public class HttpFileSizeValidateAttribute : ValidationAttribute
    {
        #region Properties

        /// <summary>
        ///     Allowed content length of the file.
        /// </summary>
        private readonly uint _contentLength;

        #endregion

        #region Constructor

        /// <summary>
        ///     Initialize attribute with default settings.
        /// </summary>
        /// <param name="bytes"></param>
        public HttpFileSizeValidateAttribute(uint bytes)
        {
            _contentLength = bytes;
        }

        #endregion

        /// <summary>
        ///     Check whether file size is valid or not.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="validationContext"></param>
        /// <returns></returns>
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            // Invalid value.
            if (value == null)
                return ValidationResult.Success;

            if (!(value is HttpFile))
                throw new Exception("Object my be an instance of HttpFileModel");

            // Cast object to HttpFileModel instance.
            var httpFile = (HttpFile) value;
            if (httpFile.Buffer.Length > _contentLength)
                return new ValidationResult(FormatErrorMessage(validationContext.DisplayName));

            return ValidationResult.Success;
        }
    }
}