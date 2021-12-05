using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data;
using Microsoft.EntityFrameworkCore;

namespace Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        DbContext Context;
        IGenericRepository<Category> CategoryRepo;
        IGenericRepository<Product> ProductRepo;
        IGenericRepository<RetailerReviewProduct> ProductReviewRepo;

        public UnitOfWork(SouqElgomlaContext context ,
                           IGenericRepository<Category> _CategoryRepo,
                           IGenericRepository<Product> _ProductRepo,
                           IGenericRepository<RetailerReviewProduct> _ProductReviewRepo)
        {
            Context = context;
            CategoryRepo = _CategoryRepo;
            ProductRepo = _ProductRepo;
            ProductReviewRepo = _ProductReviewRepo;
        }

        public IGenericRepository<Category> GetCategoryRepository()
        {
            return CategoryRepo;
        }

        public IGenericRepository<Product> GetProductRepository()
        {
            return ProductRepo;
        }

        public IGenericRepository<RetailerReviewProduct> GetProductReview()
        {
            return ProductReviewRepo;
        }

        public async Task Save()
        {
            await Context.SaveChangesAsync();
        }
    }
}
