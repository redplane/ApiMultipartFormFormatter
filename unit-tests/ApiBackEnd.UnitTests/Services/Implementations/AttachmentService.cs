﻿using System;
using System.IO;
using System.Threading.Tasks;
using ApiBackEnd.UnitTests.Services.Interfaces;
using ApiMultiPartFormData.Models;

namespace ApiBackEnd.UnitTests.Services.Implementations
{
    public class AttachmentService : IAttachmentService
    {
        public virtual Task<HttpFileBase> LoadSampleAttachmentAsync()
        {
            var encodedAttachment =
                "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";

            var bytes = Convert.FromBase64String(encodedAttachment);
            var attachment = new HttpFileBase("attachment-001.jpg", new MemoryStream(bytes), "image/jpg");
            return Task.FromResult(attachment);
        }
    }
}