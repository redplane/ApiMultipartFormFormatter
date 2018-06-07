using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ApiBackEnd.Models;
using ApiMultiPartFormData.Models;

namespace ApiBackEnd.ViewModels
{
    public class UploadAttachmentListViewModel
    {
        #region Properties

        /// <summary>
        /// Author information.
        /// </summary>
        public User Author { get; set; }

        /// <summary>
        /// List of attachments that will be uploaded to server.
        /// </summary>
        [Required]
        public List<HttpFile> Attachments { get; set; }

        #endregion
    }
}