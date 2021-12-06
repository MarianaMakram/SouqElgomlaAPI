using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class MakeOrderEntityConfiguration : IEntityTypeConfiguration<MakeOrder>
    {
        public void Configure(EntityTypeBuilder<MakeOrder> builder)
        {
            builder.ToTable("MakeOrder");
            builder.HasKey(i => i.ID);
            builder.Property(i => i.ID).ValueGeneratedOnAdd();
            builder.Property(i => i.UserId).IsRequired();
            builder.Property(i => i.OrderID).IsRequired();
        }
    }
}
