using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class Product : BaseModel
    {
        public string Name { get; set; }
        public double Price { get; set; }
        public DateTime ExpireDate { get; set; }
        public string UnitWeight { get; set; }
        public string Image { get; set; }
        public DateTime ProductionDate{ get; set; }

        public int? AdminID { get; set; }

        public int? CategoryID { get; set; }

        public int? SupplierID { get; set; }

        public virtual Admin Admin { get; set; }
        public virtual ICollection<BuyProduct> BuyProducts { get; set; }

        public virtual Category Category { get; set; }

        public virtual Supplier Supplier { get; set; }
        public virtual ICollection<RetailerReviewProduct> RetailerReviewProducts { get; set; }

    }
}
