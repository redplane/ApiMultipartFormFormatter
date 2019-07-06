using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ApiMultiPartFormData.Models;

namespace MultipartFormDataMyGet.ViewModels
{
    public class UploadAttachmentListViewModel
    {
        #region Properties


        /// <summary>
        /// List of attachments that will be uploaded to server.
        /// </summary>
        [Required]
        public List<HttpFile> Attachments { get; set; }

        #endregion
    }
}