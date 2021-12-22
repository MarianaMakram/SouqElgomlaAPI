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
        public Order order { get; set; }
        public IList<ProductOrder> productOrders { get; set; }
    }
}
