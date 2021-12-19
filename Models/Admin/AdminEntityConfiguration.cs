using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class AdminEntityConfiguration : IEntityTypeConfiguration<Admin>
    {
        public void Configure(EntityTypeBuilder<Admin> builder)
        {
            builder.ToTable("adminLogin");
            builder.HasKey(i => i.id);
            builder.Property(i => i.id).HasColumnType("varchar").ValueGeneratedOnAdd();
            builder.Property(i => i.name).IsRequired().HasColumnType("varchar").HasMaxLength(30);
            builder.Property(i => i.Address).HasColumnType("varchar").HasMaxLength(30);
            builder.Property(i => i.password).IsRequired().HasColumnType("varchar").HasMaxLength(50);
            builder.Property(i => i.email).IsRequired().HasColumnType("varchar").HasMaxLength(30);
        }
    }
}
