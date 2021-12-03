using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class SupplierEntityConfiguration : IEntityTypeConfiguration<Supplier>
    {
        public void Configure(EntityTypeBuilder<Supplier> builder)
        {
            builder.ToTable("Supplier");
            builder.HasKey(i => i.ID);
            builder.Property(i => i.ID).ValueGeneratedOnAdd();
            builder.Property(i => i.CompanyName).IsRequired().HasMaxLength(50);
            builder.Property(i => i.UserName).IsRequired().HasMaxLength(50);
            builder.Property(i => i.Password).IsRequired().HasMaxLength(50);
            builder.Property(i => i.Email).IsRequired().HasMaxLength(50);
            builder.Property(i => i.Phone).IsRequired().HasMaxLength(20);

        }
    }
}
