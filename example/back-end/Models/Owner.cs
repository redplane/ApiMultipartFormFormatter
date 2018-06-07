using ApiMultiPartFormData.Models;

namespace HttpFormatter.Test.Models
{
    public class Owner
    {
        public Name Name { get; set; }

        public HttpFile Photo { get; set; }
    }
}