using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class Product : BaseModel
    {
        public string Name { get; set; }
        public string? Description { get; set; }
        public double? Price { get; set; }
        public int? Quantity { get; set; }
        public DateTime? ExpireDate { get; set; }
        public string? UnitWeight { get; set; }
        public string? ImageUrl { get; set; }
        public byte[]? Image { get; set; }
        public DateTime? ProductionDate{ get; set; }
        public int? PackgesNumber { get; set; }

        [ForeignKey("User")]
        public string? UserId { get; set; }

        [ForeignKey("Category")]
        public int? CategoryID { get; set; }
        public bool? IsApproved { get; set; }

        public virtual User User { get; set; }
        public virtual Category Category { get; set; }
        public virtual ICollection<RetailerReviewProduct> RetailerReviewProducts { get; set; }

    }
}
