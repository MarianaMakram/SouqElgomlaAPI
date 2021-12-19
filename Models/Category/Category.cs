using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class Category : BaseModel
    {
        public string Name { get; set; }

        public string? Description { get; set; }
        public string? ImgUrl { get; set; }
        public byte[]? Image { get; set; }

        public virtual ICollection<Product> Products { get; set; }
    }
}
