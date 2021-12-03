using System;

namespace Models
{
    public class BuyProduct : BaseModel
    {
        public int ProductID { get; set; }
        public int RetailerID { get; set; }
        public DateTime? Time { get; set; }
        public int? Quantity { get; set; }
        public virtual Product Product { get; set; }
        public virtual Retailer Retailer { get; set; }
    }
}
