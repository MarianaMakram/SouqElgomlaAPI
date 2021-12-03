using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class Payment : BaseModel
    {
        public string PaymentType { get; set; }
        public string PaymentAllowed { get; set; }
    }
}
