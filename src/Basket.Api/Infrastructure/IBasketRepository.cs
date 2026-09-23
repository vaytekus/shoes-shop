using Basket.Api.Domain;

namespace Basket.Api.Infrastructure;

public interface IBasketRepository
{
    Task<CustomerBasket?> GetBasketAsync(string customerId, CancellationToken ct = default);
    Task<CustomerBasket> SaveBasketAsync(CustomerBasket basket, CancellationToken ct = default);
    Task DeleteBasketAsync(string customerId, CancellationToken ct = default);
}
