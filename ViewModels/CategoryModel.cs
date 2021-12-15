using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models;

namespace ViewModels
{
    public class CategoryModel
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string ImgUrl { get; set; }
        public List<Product> Products { get; set; }

    }

    public static class CategoryViewModelExtention
    {
        public static CategoryModel ToCategoryModel(this Category category)
        {
            var model = new CategoryModel
            {
                ID = category.ID,
                Name = category.Name,
                ImgUrl = category.ImgUrl,
                Products = category.Products.ToList()
            };

            return model;
        }
    }
}
