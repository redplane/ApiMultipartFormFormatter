using System;
using System.Collections.Generic;
using ApiBackEnd.Enumerations;
using ApiMultiPartFormData.Models;

namespace ApiBackEnd.UnitTests.ViewModels
{
    public class RawProfileViewModel
    {
        public List<string> Ids { get; set; }

        public List<string> Qualities { get; set; }

        public string NonNullableId { get; set; }

        public string NullableId { get; set; }

        public string Name { get; set; }

        public string Age { get; set; }

        public string Amount { get; set; }

        public string NullableQuality { get; set; }

        public string NonNullableQuality { get; set; }

        public HttpFileBase Photo { get; set; }

        public List<HttpFileBase> Photos { get; set; }
    }
}
