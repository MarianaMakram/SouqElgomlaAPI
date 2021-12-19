using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class ProductEntityConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.ToTable("Product");
            builder.HasKey(i => i.ID);
            builder.Property(i => i.ID).ValueGeneratedOnAdd();
            builder.Property(i => i.Name).IsRequired(true).HasMaxLength(50);
            builder.Property(i => i.ImageUrl).HasMaxLength(200);
            builder.Property(i => i.Price).IsRequired(true);
            builder.Property(i => i.ProductionDate).HasColumnType("date");
            builder.Property(i => i.ExpireDate).HasColumnType("date");
            builder.Property(i => i.UnitWeight).HasMaxLength(50);
            builder.Property(i => i.UserId).IsRequired(false);
            builder.Property(i => i.CategoryID).IsRequired(false);
            builder.Property(i => i.Description).HasMaxLength(1000);
        }
    }
}
