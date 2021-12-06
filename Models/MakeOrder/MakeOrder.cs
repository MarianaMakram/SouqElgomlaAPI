using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class MakeOrder : BaseModel
    {
        [ForeignKey("User")]
        public string UserId { get; set; }

        [ForeignKey("Order")]
        public int OrderID { get; set; }

        public virtual Order Order { get; set; }

        public virtual User User { get; set; }
    }
}
