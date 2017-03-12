using System;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.IO;
using MultiPartFormData.Models;

namespace MultiPartFormData.Attributes
{
    public class HttpFileImageValidateAttribute : ValidationAttribute
    {
        #region Methods

        /// <summary>
        ///     Check whether regular expression is valid or not.
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
                throw new Exception("Object is invalid.");

            // Cast object to HttpFileModel instance.
            var httpFile = (HttpFile) value;

            #region Bytestream validate

            try
            {
                using (var memoryStream = new MemoryStream(httpFile.Buffer))
                {
                    memoryStream.Seek(0, SeekOrigin.Begin);
                    Image.FromStream(memoryStream);

                    return ValidationResult.Success;
                }
            }
            catch
            {
                return new ValidationResult(FormatErrorMessage(validationContext.DisplayName));
            }

            #endregion
        }

        #endregion
    }
}