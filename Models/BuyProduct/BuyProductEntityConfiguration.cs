using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class BuyProductEntityConfiguration : IEntityTypeConfiguration<BuyProduct>
    {
        public void Configure(EntityTypeBuilder<BuyProduct> builder)
        {
            builder.ToTable("BuyProduct");
            builder.HasKey(i => i.ID);
            builder.Property(i => i.ID).ValueGeneratedOnAdd();
            builder.Property(i => i.UserId).IsRequired();
            builder.Property(i => i.ProductID).IsRequired();
            builder.Property(i => i.Time).HasColumnType("date");
        }
    }
}
