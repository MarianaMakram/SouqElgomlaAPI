using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models
{
    public class BuyProduct : BaseModel
    {
        [ForeignKey("Product")]
        public int ProductID { get; set; }

        [ForeignKey("User")]
        public string UserId { get; set; }
        public DateTime? Time { get; set; }
        public int? Quantity { get; set; }
        public virtual Product Product { get; set; }
        public virtual User User { get; set; }
    }
}
