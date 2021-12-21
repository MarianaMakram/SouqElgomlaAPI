using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class ProductOrder : BaseModel
    {
        [ForeignKey("Order")]
        public int OrderID { get; set; }

        public virtual Order Order { get; set; }

        [ForeignKey("Product")]
        public int ProductID { get; set; }
        public virtual Product Product { get; set; }

        public int Quantity { get; set; }
    }
}
