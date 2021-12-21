using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class RetailerReviewProductEntityConfiguration : IEntityTypeConfiguration<RetailerReviewProduct>
    {
        public void Configure(EntityTypeBuilder<RetailerReviewProduct> builder)
        {
            builder.ToTable("RetailerReviewProduct");
            builder.HasKey(i => i.ID);
            builder.Property(i => i.ID).ValueGeneratedOnAdd();
            builder.Property(i => i.Time).HasColumnType("date");
        }
    }
}
