using System;
using System.ComponentModel.DataAnnotations;
using ApiBackEnd.Models;
using ApiMultiPartFormData.Models;

namespace ApiBackEnd.ViewModels
{
    public class BasicUploadStreamedViewModel
    {
        #region Properties

        public Guid Id { get; set; }

        /// <summary>
        /// Author information.
        /// </summary>
        [Required]
        public User Author { get; set; }

        /// <summary>
        /// Attachment.
        /// </summary>
        [Required]
        public StreamedHttpFile StreamedAttachment { get; set; }

        #endregion
    }
}