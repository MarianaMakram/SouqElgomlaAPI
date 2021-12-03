using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class RetailerEntityConfiguration : IEntityTypeConfiguration<Retailer>
    {
        public void Configure(EntityTypeBuilder<Retailer> builder)
        {
            builder.ToTable("Retailer");
            builder.HasKey(i => i.ID);
            builder.Property(i => i.ID).ValueGeneratedOnAdd();
            builder.Property(i => i.FirstName).IsRequired().HasMaxLength(50);
            builder.Property(i => i.LastName).IsRequired().HasMaxLength(50);
            builder.Property(i => i.UserName).IsRequired().HasMaxLength(50);
            builder.Property(i => i.Password).IsRequired().HasMaxLength(50);
            builder.Property(i => i.Address).HasMaxLength(50);
            builder.Property(i => i.City).HasMaxLength(20);
            builder.Property(i => i.Image).HasMaxLength(200);
            builder.Property(i => i.Phone).HasMaxLength(20);

        }
    }
}
