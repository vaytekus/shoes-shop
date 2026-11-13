using MassTransit;
using MediatR;
using Order.Api.Domain;
using Order.Api.Infrastructure;
using ShoesShop.Shared.Events;

namespace Order.Api.Features.Orders.Commands.CreateOrder;

public class CreateOrderHandler(OrderDbContext db, IPublishEndpoint publishEndpoint) : IRequestHandler<CreateOrderCommand, Guid>
{
    public async Task<Guid> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        var order = new CustomerOrder
        {
            Id = Guid.NewGuid(),
            FirstName = request.FirstName,
            LastName = request.LastName,
            Phone = request.Phone,
            Email = request.Email,
            DeliveryType = request.DeliveryType,
            PaymentMethod = request.PaymentMethod,
            CustomerId = request.CustomerId,
            Status = OrderStatus.Pending,
            ShippingAddress = new ShippingAddress
            {
                Street = request.ShippingAddress.Street, City = request.ShippingAddress.City, Country = request.ShippingAddress.Country, ZipCode = request.ShippingAddress.ZipCode
            },
            Items = request.Items.Select(i => new OrderItem
            {
                Id = Guid.NewGuid(),
                ProductId = i.ProductId,
                ProductName = i.ProductName,
                Price = i.Price,
                Quantity = i.Quantity,
                ImageUrl = i.ImageUrl,
                Size = i.Size
            }).ToList(),
            TotalAmount = request.Items.Sum(i => i.Price * i.Quantity),
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
        };

        db.Orders.Add(order);
        await db.SaveChangesAsync(cancellationToken);

        await publishEndpoint.Publish(new OrderCreatedEvent(order.Id, order.CustomerId), cancellationToken);

        return order.Id;
    }
}
