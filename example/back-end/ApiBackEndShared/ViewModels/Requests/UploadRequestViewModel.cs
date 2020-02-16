using System;
using System.Collections.Generic;
using ApiBackEndShared.Enumerations;
using ApiBackEndShared.Interfaces;
using ApiMultiPartFormData.Models;

namespace ApiBackEndShared.ViewModels.Requests
{
    public class UploadRequestViewModel
    {
        #region Properties

        public Guid Id { get; set; }

        public Guid? AttachmentId { get; set; }

        /// <summary>
        ///     Author information.
        /// </summary>
        public ProfileViewModel Profile { get; set; }

        /// <summary>
        ///     Attachment.
        /// </summary>
        public HttpFileBase Attachment { get; set; }

        public List<HttpFile> Attachments { get; set; }

        public Qualities NonNullableQuality { get; set; }

        public Qualities? NullableQuality { get; set; }

        public List<Qualities> Qualities { get; set; }

        public List<Guid> Ids { get; set; }

        #endregion
    }
}