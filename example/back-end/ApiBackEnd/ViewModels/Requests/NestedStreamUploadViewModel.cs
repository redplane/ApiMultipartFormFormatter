using System;
using System.ComponentModel.DataAnnotations;
using ApiBackEnd.Models;
using ApiMultiPartFormData.Models;

namespace ApiBackEnd.ViewModels.Requests
{
    public class NestedStreamUploadViewModel
    {
        #region Properties

        public Guid? Id { get; set; }

        public HttpFile Attachment { get; set; }

        public StreamProfileViewModel Profile { get; set; }

        public bool IsActive { get; set; }

        #endregion
    }
}