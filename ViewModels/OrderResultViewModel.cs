using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels
{
    public class OrderResultViewModel
    {
        public int orderId { get; set; }
        public string userId { get; set; }
        public DateTime orderDate { get; set; }
        public int state { get; set; }
        public int paymentType { get; set; }
        public string name { get; set; }
        public string address { get; set; }
        public string phone { get; set; }
        public IList<ProductOrderViewModel> productOrderViewModels { get; set; }
    }
}
