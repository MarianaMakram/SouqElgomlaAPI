using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels
{
    public class PostOrderModel
    {
        public string name { get; set; }
        public string address { get; set; }
        public string phone { get; set; }
        public int payment { get; set; }

        public OrderviewModel[] orderviewModels { get; set; }
    }
}
