using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class SupplierRetailerReviewEntityConfiguration
        : IEntityTypeConfiguration<SupplierRetailerReview>
    {
        public void Configure(EntityTypeBuilder<SupplierRetailerReview> builder)
        {
            builder.ToTable("SupplierRetailerReview");
            builder.HasKey(i => i.ID);
            builder.Property(i => i.ID).ValueGeneratedOnAdd();
            builder.Property(i => i.RetailerID).IsRequired();
            builder.Property(i => i.SupplierID).IsRequired();
            builder.Property(i => i.Rate).IsRequired();
            builder.Property(i => i.Time).HasColumnType("date");

        }
    }
}
