using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class Order : BaseModel
    {
        public bool? IsComplete { get; set; }

        [ForeignKey("Shipper")]
        public int? ShipperID { get; set; }

        [ForeignKey("Payment")]
        public int? PaymentID { get; set; }

        public virtual ICollection<MakeOrder> MakeOrders { get; set; }

        public virtual Shipper Shipper { get; set; }
        public virtual Payment Payment { get; set; }
    }
}
