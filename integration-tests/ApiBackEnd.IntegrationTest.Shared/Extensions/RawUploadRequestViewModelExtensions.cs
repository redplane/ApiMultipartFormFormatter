﻿using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using ApiBackEnd.IntegrationTest.Shared.ViewModels;

namespace ApiBackEnd.IntegrationTest.Shared.Extensions
{
    public static class RawUploadRequestViewModelExtensions
    {
        public static MultipartFormDataContent ToMultipartFormDataContent(this RawUploadRequestViewModel model)
        {
            var multipartFormDataContent = new MultipartFormDataContent($"-www-www-{DateTime.Now:yy-MM-dd}-www-www-");

            if (!string.IsNullOrEmpty(model.Id))
                multipartFormDataContent.Add(new StringContent(model.Id, Encoding.UTF8), nameof(model.Id));

            if (!string.IsNullOrEmpty(model.AttachmentId))
                multipartFormDataContent.Add(new StringContent(model.AttachmentId, Encoding.UTF8),
                    nameof(model.AttachmentId));

            if (model.Attachment != null)
            {
#if NETFRAMEWORK
                multipartFormDataContent.Add(model.Attachment
                    .ToByteArrayContent(nameof(model.Attachment)));
#elif NETSTANDARD
                var content = new ByteArrayContent(model.Attachment.ToBytes());
                content.Headers.ContentType = MediaTypeHeaderValue.Parse(model.Attachment.ContentType);
                multipartFormDataContent.Add(content,
                nameof(model.Attachment), model.Attachment.FileName);
#endif
            }

            if (model.Attachments != null && model.Attachments.Count > 0)
            {
                for (var attachmentId = 0; attachmentId < model.Attachments.Count; attachmentId++)
                {
#if NETFRAMEWORK
                    multipartFormDataContent.Add(model.Attachments[attachmentId]
                        .ToByteArrayContent($"{nameof(model.Attachments)}[{attachmentId}]"));
#elif NETSTANDARD
                    var attachment = model.Attachments[attachmentId];
                    var content = new ByteArrayContent(attachment.ToBytes());
                    content.Headers.ContentType = MediaTypeHeaderValue.Parse(attachment.ContentType);
                    multipartFormDataContent.Add(content,
                    $"{nameof(model.Attachments)}[{attachmentId}]", attachment.FileName);
#endif
                }
            }

            var nonNullableQuality = string.IsNullOrEmpty(model.NonNullableQuality) ? "" : model.NonNullableQuality;
            multipartFormDataContent.Add(new StringContent(nonNullableQuality, Encoding.UTF8),
                nameof(model.NonNullableQuality));

            var nullableQualityContent = string.IsNullOrEmpty(model.NullableQuality) ? "" : model.NullableQuality;
            multipartFormDataContent.Add(new StringContent(nullableQualityContent, Encoding.UTF8),
                nameof(model.NullableQuality));

            if (model.Ids != null)
                for (var id = 0; id < model.Ids.Count; id++)
                    multipartFormDataContent.Add(new StringContent(model.Ids[id], Encoding.UTF8),
                        $"{nameof(model.Ids)}[{id}]");

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