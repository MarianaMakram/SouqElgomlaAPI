using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models;

namespace ViewModels
{
    public class ProductModel
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public double Price { get; set; }
        public string? UnitWeight { get; set; }
        public string Image { get; set; }
        public int? Rate { get; set; }
        public int Quantity { get; set; }

    }

    public static class ProductModelExtention
    {
        public static ProductModel ToProductModel(this Product product,int? rate)
        {
            return new ProductModel
            {
                ID = product.ID,
                Name = product.Name,
                Image = product.Image,
                Price = product.Price,
                Description = product.Description,
                UnitWeight = product.UnitWeight,
                Rate = rate,
                Quantity = product.Quantity
            };
        }
    }
}
