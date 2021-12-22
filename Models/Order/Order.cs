using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public enum OrderDeliveredState
    {
        Pending,
        OnTheWay,
        Delivered
    }
    public class Order : BaseModel
    {

        [ForeignKey("User")]
        public string UserId { get; set; }
        public virtual User User { get; set; }

        public DateTime OrderDate { get; set; }

        public OrderDeliveredState State { set; get; }
        public virtual ICollection<ProductOrder> ProductOrders { get; set; }

        #region old entity
        //public bool? IsComplete { get; set; }

        //[ForeignKey("Shipper")]
        //public int? ShipperID { get; set; }

        //[ForeignKey("Payment")]
        //public int? PaymentID { get; set; }

        //public virtual ICollection<MakeOrder> MakeOrders { get; set; }

        //public virtual Shipper Shipper { get; set; }
        //public virtual Payment Payment { get; set; }
        #endregion
    }
}
