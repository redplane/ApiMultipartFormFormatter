using System;
using System.IO;
using System.Net.Http;
using System.Text;
using ApiBackEnd.UnitTests.ViewModels;
using ApiBackEnd.ViewModels.Requests;

namespace ApiBackEnd.UnitTests.Extensions
{
    public static class RawUploadRequestViewModelExtensions
    {
        public static MultipartFormDataContent ToMultipartFormDataContent(this RawUploadRequestViewModel model)
        {
            var multipartFormDataContent = new MultipartFormDataContent($"-www-www-{DateTime.Now:yy-MM-dd}-www-www-");

            if (!string.IsNullOrEmpty(model.Id))
            {
                multipartFormDataContent.Add(new StringContent(model.Id, Encoding.UTF8), nameof(model.Id));
            }

            if (!string.IsNullOrEmpty(model.AttachmentId))
            {
                multipartFormDataContent.Add(new StringContent(model.AttachmentId, Encoding.UTF8), nameof(model.AttachmentId));
            }

            if (model.Attachment != null)
            {
                multipartFormDataContent.Add(model.Attachment
                    .ToByteArrayContent(nameof(model.Attachment)));
            }

            if (model.Attachments != null && model.Attachments.Count > 0)
            {
                for (var attachmentId = 0; attachmentId < model.Attachments.Count; attachmentId++)
                {
                    multipartFormDataContent.Add(model.Attachments[attachmentId]
                        .ToByteArrayContent($"{nameof(model.Attachments)}[{attachmentId}]"));
                }
            }

            if (!string.IsNullOrEmpty(model.NonNullableQuality))
            {
                multipartFormDataContent.Add(new StringContent(model.NonNullableQuality, Encoding.UTF8), nameof(model.NonNullableQuality));
            }

            if (!string.IsNullOrEmpty(model.NullableQuality))
            {
                multipartFormDataContent.Add(new StringContent(model.NullableQuality, Encoding.UTF8), nameof(model.NullableQuality));
            }

            if (model.Ids != null)
            {
                for (var id = 0; id < model.Ids.Count; id++)
                {
                    multipartFormDataContent.Add(new StringContent(model.Ids[id], Encoding.UTF8), $"{nameof(model.Ids)}[{id}]");
                }
            }

            if (model.Qualities != null && model.Qualities.Count > 0)
            {
                for (var id = 0; id < model.Qualities.Count; id++)
                    multipartFormDataContent
                        .Add(new StringContent(model.Qualities[id], Encoding.UTF8), $"{nameof(model.Qualities)}[{id}]");
            }

            model.Profile?.ExtendHttpContent(multipartFormDataContent, $"{nameof(model.Profile)}.");

            return multipartFormDataContent;
        }
    }
}
