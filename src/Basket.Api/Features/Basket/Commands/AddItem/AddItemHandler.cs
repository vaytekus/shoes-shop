using Basket.Api.Domain;
using Basket.Api.Infrastructure;
using MediatR;

namespace Basket.Api.Features.Basket.Commands.AddItem;

public class AddItemHandler(IBasketRepository repository) : IRequestHandler<AddItemCommand, CustomerBasket>
{
    public async Task<CustomerBasket> Handle(AddItemCommand request, CancellationToken ct)
    {
        var basket = await repository.GetBasketAsync(request.CustomerId, ct)
            ?? new CustomerBasket
            {
                CustomerId = request.CustomerId
            };

        var existing = basket.Items
            .FirstOrDefault(b => b.ProductId == request.ProductId);

        if (existing is not null)
        {
            existing.Quantity += request.Quantity;
        }
        else
        {
            basket.Items.Add(new BasketItem
            {
                ProductId = request.ProductId,
                ProductName = request.ProductName,
                Price = request.Price,
                Quantity = request.Quantity,
                ImageUrl = request.ImageUrl
            });
        }

        return await repository.SaveBasketAsync(basket, ct);
    }
}
