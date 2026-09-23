using Basket.Api.Infrastructure;
using MediatR;

namespace Basket.Api.Features.Basket.Commands.RemoveItem;

public class RemoveItemHandler(IBasketRepository repository) : IRequestHandler<RemoveItemCommand>
{
    public async Task Handle(RemoveItemCommand request, CancellationToken ct)
    {
        var basket = await repository.GetBasketAsync(request.CustomerId);

        if (basket is null)
        {
            return;
        }

        basket.Items
            .RemoveAll(i => i.ProductId == request.ProductId);

        await repository.SaveBasketAsync(basket, ct);
    }
}
