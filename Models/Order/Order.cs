using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class Order : BaseModel
    {
        public bool? IsComplete { get; set; }
        public int? ShipperID { get; set; }
        public int? PaymentID { get; set; }

        public virtual ICollection<MakeOrder> MakeOrders { get; set; }

        public virtual Shipper Shipper { get; set; }
        public virtual Payment Payment { get; set; }
    }
}
