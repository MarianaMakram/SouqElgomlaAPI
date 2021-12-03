using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class SupplierRetailerReview : BaseModel
    {
        public int RetailerID { get; set; }
        public int SupplierID { get; set; }
        public DateTime? Time { get; set; }
        public int Rate { get; set; }

        public virtual Retailer Retailer { get; set; }

        public virtual Supplier Supplier { get; set; }
    }
}
