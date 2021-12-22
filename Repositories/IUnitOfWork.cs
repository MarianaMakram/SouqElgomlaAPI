using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models;

namespace Repositories
{
    public interface IUnitOfWork
    {
        IGenericRepository<Category> GetCategoryRepository();
        IGenericRepository<Product> GetProductRepository();
        IGenericRepository<RetailerReviewProduct> GetProductReview();
        IGenericRepository<Order> GetOrderRepository();
        IGenericRepository<ProductOrder> GetProductOrderRepository();
        IUserRepository GetUserRepository();
        Task Save();
    }
}
