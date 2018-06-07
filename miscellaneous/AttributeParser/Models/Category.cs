using System.Collections.Generic;

namespace AttributeParser.Models
{
    public class Category
    {
        public int Id { get; set; }
        
        public List<Photo> Photos { get; set; }
    }
}