using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Order.Api.Domain;

namespace Order.Api.Configurations;

public class OrderConfiguration : IEntityTypeConfiguration<CustomerOrder>
{
    public void Configure(EntityTypeBuilder<CustomerOrder> builder)
    {
        builder.HasKey(o => o.Id);

        builder.Property(o => o.CustomerId).IsRequired();
        builder.Property(o => o.TotalAmount).HasPrecision(18, 2);
        builder.Property(o => o.Status).HasConversion<string>();

        builder.OwnsOne(o => o.ShippingAddress, sa => {
            sa.Property(a => a.Street).IsRequired();
            sa.Property(a => a.City).IsRequired();
            sa.Property(a => a.Country).IsRequired();
            sa.Property(a => a.ZipCode).IsRequired();
        });

        builder.HasMany(o => o.Items)
            .WithOne()
            .HasForeignKey(i => i.OrderId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
