using System;
using System.Collections.Generic;
using ApiBackEnd.Enumerations;
using ApiMultiPartFormData.Models;

namespace ApiBackEnd.ViewModels
{
    public class ProfileViewModel
    {
        public List<Guid> Ids { get; set; }

        public List<Qualities> Qualities { get; set; }

        public Guid NonNullableId { get; set; }

        public Guid? NullableId { get; set; }

        public string Name { get; set; }

        public int Age { get; set; }

        public decimal Amount { get; set; }

        public Qualities? NullableQuality { get; set; }

        public Qualities NonNullableQuality { get; set; }

        public HttpFileBase Photo { get; set; }

        public List<HttpFileBase> Photos { get; set; }
    }
}