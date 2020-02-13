using System;
using System.Collections.Generic;
using System.Linq;
using ApiBackEnd.Enumerations;
using ApiBackEnd.ViewModels.Requests;

namespace ApiBackEnd.ViewModels.Responses
{
    public class UploadResponseViewModel
    {
        #region Properties

        public Guid Id { get; set; }

        public Guid? AttachmentId { get; set; }

        public ProfileResponseViewModel Profile { get; set; }

        public HttpFileBaseResponseViewModel Attachment { get; set; }

        public List<HttpFileBaseResponseViewModel> Attachments { get; set; }

        public List<Guid> Ids { get; set; }

        public Qualities NonNullableQuality { get; set; }

        public Qualities? NullableQuality { get; set; }

        public List<Qualities> Qualities { get; set; }

        #endregion

        #region Constructor

        public UploadResponseViewModel()
        {
        }

        public UploadResponseViewModel(UploadRequestViewModel model)
        {
            Id = model.Id;
            AttachmentId = model.AttachmentId;

            if (model.Profile != null)
                Profile = new ProfileResponseViewModel(model.Profile);

            if (model.Attachment != null)
                Attachment = new HttpFileBaseResponseViewModel(model.Attachment);

            if (model.Attachments != null && model.Attachments.Count > 0)
                Attachments = model.Attachments.Select(attachment => new HttpFileBaseResponseViewModel(attachment))
                    .ToList();

            Ids = model.Ids;
            NonNullableQuality = model.NonNullableQuality;
            NullableQuality = model.NullableQuality;
            Qualities = model.Qualities;
        }

        #endregion
    }
}