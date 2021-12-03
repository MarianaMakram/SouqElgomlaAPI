using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class Supplier : BaseModel
    {
        public string CompanyName { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public int? AdminID { get; set; }

        public virtual Admin Admin { get; set; }
        public virtual ICollection<Product> Products { get; set; }
        public virtual ICollection<SupplierRetailerReview> SupplierRetailerReviews { get; set; }

    }
}
