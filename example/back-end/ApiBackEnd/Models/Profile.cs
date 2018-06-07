using ApiMultiPartFormData.Models;

namespace ApiBackEnd.Models
{
    public class Profile
    {
        public string Name { get; set; }
        
        public HttpFile Attachment { get; set; }
    }
}