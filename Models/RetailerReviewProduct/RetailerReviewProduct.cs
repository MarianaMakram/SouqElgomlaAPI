using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class RetailerReviewProduct : BaseModel
    {
        [ForeignKey("Product")]
        public int ProductID { get; set; }

        [ForeignKey("User")]
        public string UserId { get; set; }
        public DateTime? Time { get; set; }
        public int? Rate { get; set; }

        public virtual Product Product { get; set; }

        public virtual User User { get; set; }
    }
}
