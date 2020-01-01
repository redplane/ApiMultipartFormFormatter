using ApiMultiPartFormData.Models;

namespace ApiBackEnd.ViewModels
{
    public class StreamProfileViewModel
    {
        public string Name { get; set; }

        public HttpFileBase Attachment { get; set; }
    }
}