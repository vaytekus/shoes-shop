using Basket.Api.Infrastructure;
using MassTransit;
using ShoesShop.Shared.Events;

namespace Basket.Api.Consumers;

public class OrderCreatedConsumer(IBasketRepository repository) : IConsumer<OrderCreatedEvent>
{
    public async Task Consume(ConsumeContext<OrderCreatedEvent> context)
    {
        await repository.DeleteBasketAsync(context.Message.CustomerId, context.CancellationToken);
    }
}
