using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class RetailerReviewProduct : BaseModel
    {
        public int ProductID { get; set; }
        public int RetailerID { get; set; }
        public DateTime? Time { get; set; }
        public int? Rate { get; set; }

        public virtual Product Product { get; set; }

        public virtual Retailer Retailer { get; set; }
    }
}
