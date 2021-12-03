using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class Admin :BaseModel
    {
        public string Name { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }        
        public virtual ICollection<Product> Products { get; set; }
        public virtual ICollection<Retailer> Retailers { get; set; }
        public virtual ICollection<Supplier> Suppliers { get; set; }
    }
}
