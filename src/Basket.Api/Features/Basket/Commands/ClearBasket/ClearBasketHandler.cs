using Basket.Api.Infrastructure;
using MediatR;

namespace Basket.Api.Features.Basket.Commands.ClearBasket;

public class ClearBasketHandler(IBasketRepository repository) : IRequestHandler<ClearBasketCommand>
{
    public async Task Handle(ClearBasketCommand request, CancellationToken ct)
    {
        await repository.DeleteBasketAsync(request.CustomerId, ct);
    }
}
