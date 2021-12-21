using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class User : IdentityUser
    {
        
        public string Name { get; set; }
        public string? Address { get; set; }
        public string? Image { get; set; }
        public string? PayPalAccount { get; set; }

        //public byte[]? Image { get; set; }

        public virtual ICollection<Product> Products { get; set; }
        public virtual ICollection<RetailerReviewProduct> RetailerReviewProducts { get; set; }
        public virtual ICollection<Order> Orders { get; set; }

        //public virtual ICollection<BuyProduct> BuyProducts { get; set; }
        //public virtual ICollection<MakeOrder> MakeOrders { get; set; }

    }
}
