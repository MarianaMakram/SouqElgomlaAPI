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

        public UnitOfWork(SouqElgomlaContext context ,
                           IGenericRepository<Category> _CategoryRepo,
                           IGenericRepository<Product> _ProductRepo)
        {
            Context = context;
            CategoryRepo = _CategoryRepo;
            ProductRepo = _ProductRepo;
        }

        public IGenericRepository<Category> GetCategoryRepository()
        {
            return CategoryRepo;
        }

        public IGenericRepository<Product> GetProductRepository()
        {
            return ProductRepo;
        }

        public async Task Save()
        {
            await Context.SaveChangesAsync();
        }
    }
}
