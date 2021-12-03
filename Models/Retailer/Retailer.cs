using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class Retailer : BaseModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Phone { get; set; }
        public string Image { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public int? AdminID { get; set; }

        public virtual Admin Admin { get; set; }
        public virtual ICollection<BuyProduct> BuyProducts { get; set; }
        public virtual ICollection<MakeOrder> MakeOrders { get; set; }
        public virtual ICollection<RetailerReviewProduct> RetailerReviewProducts { get; set; }
        public virtual ICollection<SupplierRetailerReview> SupplierRetailerReviews { get; set; }
    }
}
