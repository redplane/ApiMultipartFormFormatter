using System;
using System.Collections.Generic;
using ApiMultiPartFormData.Models;

namespace ApiMultiPartFormData.UnitTest.ViewModels
{
    public class ProfileViewModel
    {
        public Guid Id { get; set; }

        public Guid? ProfileId { get; set; }

        public List<Guid> RelativeIds { get; set; }

        public string Name { get; set; }

        public HttpFileBase Attachment { get; set; }

        public HttpFile BufferedAttachment { get; set; }

        public List<HttpFileBase> Photos { get; set; }
    }
}
