using System.ComponentModel.DataAnnotations;
using ApiBackEnd.Models;
using ApiMultiPartFormData.Models;

namespace ApiBackEnd.ViewModels
{
    public class UploadNestedInfoViewModel
    {
        [Required]
        public HttpFile Attachment { get; set; }

        public Profile Profile { get; set; }
    }
}