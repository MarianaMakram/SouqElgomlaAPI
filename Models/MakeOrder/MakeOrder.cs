using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class MakeOrder : BaseModel
    {
        public int RetailerID { get; set; }

        public int OrderID { get; set; }

        public DateTime Time { get; set; }

        public virtual Order Order { get; set; }

        public virtual Retailer Retailer { get; set; }
    }
}
