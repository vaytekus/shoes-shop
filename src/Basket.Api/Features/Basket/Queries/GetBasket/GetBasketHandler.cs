using Basket.Api.Domain;
using Basket.Api.Infrastructure;
using MediatR;

namespace Basket.Api.Features.Basket.Queries.GetBasket;

public class GetBasketHandler(IBasketRepository repository) : IRequestHandler<GetBasketQuery, CustomerBasket?>
{
    public async Task<CustomerBasket?> Handle(GetBasketQuery request, CancellationToken ct)
    {
        return await repository.GetBasketAsync(request.CustomerId, ct);
    }
}
