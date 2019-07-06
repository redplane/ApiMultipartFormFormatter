using System.ComponentModel.DataAnnotations;
using ApiMultiPartFormData.Models;

namespace MultipartFormDataMyGet.ViewModels
{
    public class UploadNestedInfoViewModel
    {
        [Required]
        public HttpFile Attachment { get; set; }

        public bool IsActive { get; set; }
    }
}