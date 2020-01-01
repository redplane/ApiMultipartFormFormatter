using System;
using ApiBackEnd.Models;
using ApiMultiPartFormData.Models;

namespace ApiBackEnd.ViewModels.Requests
{
    public class FullDemoRequestViewModel
    {
        public Guid Id { get; set; }

        public Guid? NullableId { get; set; }

        public StreamProfileViewModel StreamedProfile { get; set; }

        public HttpFileBase FileBase { get; set; }
    }
}