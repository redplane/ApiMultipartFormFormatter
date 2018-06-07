using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ApiMultiPartFormData.Models;
using HttpFormatter.Test.Models;

namespace HttpFormatter.Test.ViewModels
{
    public class AccountRegistrationViewModel
    {
        /// <summary>
        /// Account owner.
        /// </summary>
        [Required]
        public Owner Owner { get; set; }

        /// <summary>
        /// User age.
        /// </summary>
        [Required]
        public int Age { get; set; }

        /// <summary>
        /// Account photo.
        /// </summary>
        [Required]
        public HttpFile Photos { get; set; }
    }
}