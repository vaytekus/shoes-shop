using Microsoft.EntityFrameworkCore;
using Order.Api.Domain;

namespace Order.Api.Infrastructure;

public class OrderDbContext(DbContextOptions<OrderDbContext> options) : DbContext(options)
{
    public DbSet<CustomerOrder> Orders => Set<CustomerOrder>();
    public DbSet<OrderItem> OrderItems => Set<OrderItem>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(OrderDbContext).Assembly);
    }
}
