using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class Shipper : BaseModel
    {
        public string CompanyName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
    }
}
