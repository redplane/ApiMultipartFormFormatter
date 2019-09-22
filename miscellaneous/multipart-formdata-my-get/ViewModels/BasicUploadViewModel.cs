using System;
using System.ComponentModel.DataAnnotations;
using ApiMultiPartFormData.Models;
using MultipartFormDataMyGet.Enums;

namespace MultipartFormDataMyGet.ViewModels
{
    public class BasicUploadViewModel
    {
        #region Properties

        public Guid Id { get; set; }

        public Guid? AttachmentId { get; set; }

        /// <summary>
        /// Attachment.
        /// </summary>
        [Required]
        public HttpFile Attachment { get; set; }

        public StudentTypes Type { get; set; }

        #endregion
    }
}