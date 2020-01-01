using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ApiBackEnd.Enumerations;
using ApiBackEnd.Models;
using ApiMultiPartFormData.Models;

namespace ApiBackEnd.ViewModels.Requests
{
    public class StreamBasicUploadViewModel
    {
        #region Properties

        public Guid Id { get; set; }

        public Guid? UserId { get; set; }

        public List<StudentTypes> StudentTypes { get; set; }

        /// <summary>
        /// Author information.
        /// </summary>
        [Required]
        public User Author { get; set; }

        /// <summary>
        /// Attachment.
        /// </summary>
        [Required]
        public HttpFileBase Attachment { get; set; }

        public Dictionary<string, string> AdditionalInfo { get; set; }

        #endregion
    }
}