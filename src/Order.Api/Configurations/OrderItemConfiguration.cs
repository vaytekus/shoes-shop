using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Order.Api.Domain;

namespace Order.Api.Configurations;

public class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
{
    public void Configure(EntityTypeBuilder<OrderItem> builder)
    {
        builder.HasKey(i => i.Id);

        builder.Property(i => i.Price).HasPrecision(18, 4);
        builder.Property(i => i.ProductName).IsRequired();
    }
}
