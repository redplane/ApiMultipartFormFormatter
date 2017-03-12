using System.Web.Http.ModelBinding;

namespace HttpFormatter.Test.Models
{
    [ModelBinder(typeof(Owner), Name = nameof(Owner))]
    public class Owner
    {
        public Name Name { get; set; }
    }
}