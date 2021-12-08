﻿using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data
{
    public class SouqElgomlaContext : IdentityDbContext<User>
    {
        public SouqElgomlaContext(DbContextOptions<SouqElgomlaContext> options) : base(options) { }

        //public DbSet<User> Users;
        public DbSet<Product> Products;
        public DbSet<Category> Categories;
        public DbSet<Shipper> Shippers;
        public DbSet<Order> Orders;
        public DbSet<Payment> Payments ;
        public DbSet<BuyProduct> BuyProducts ;
        public DbSet<MakeOrder> MakeOrders ;
        public DbSet<RetailerReviewProduct> RetailerReviewProducts ;
        //public DbSet<SupplierRetailerReview> SupplierRetailerReviews ;


        /**to link betwwen class of entity model and its confguration and Dbcontext*/
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new BuyProductEntityConfiguration());
            modelBuilder.ApplyConfiguration(new CategoryEntityConfiguration());
            modelBuilder.ApplyConfiguration(new MakeOrderEntityConfiguration());
            modelBuilder.ApplyConfiguration(new OrderEntityConfiguration());
            modelBuilder.ApplyConfiguration(new PaymentEntityConfiguration());
            modelBuilder.ApplyConfiguration(new ProductEntityConfiguration());
            modelBuilder.ApplyConfiguration(new RetailerReviewProductCategoryEntityConfiguration());
            modelBuilder.ApplyConfiguration(new ShipperEntityConfiguration());
            //modelBuilder.ApplyConfiguration(new SupplierRetailerReviewEntityConfiguration());
            modelBuilder.ApplyConfiguration(new UserEntityConfiguration());
            base.OnModelCreating(modelBuilder);
        }
        }
}