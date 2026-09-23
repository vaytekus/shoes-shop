using Basket.Api.Domain;
using Basket.Api.Infrastructure;
using MediatR;

namespace Basket.Api.Features.Basket.Commands.UpdateItem;

public class UpdateItemHandler(IBasketRepository repository) : IRequestHandler<UpdateItemCommand, CustomerBasket>
{
    public async Task<CustomerBasket> Handle(UpdateItemCommand request, CancellationToken ct)
    {
        var basket = await repository.GetBasketAsync(request.CustomerId, ct)
            ?? throw new KeyNotFoundException($"Basket for customer {request.CustomerId} not found");

        var item = basket.Items.FirstOrDefault(i => i.ProductId == request.ProductId)
            ?? throw new KeyNotFoundException($"Product {request.ProductId} not found in basket");

        item.ProductName = request.ProductName;
        item.Price = request.Price;
        item.Quantity = request.Quantity;
        item.ImageUrl = request.ImageUrl;

        return await repository.SaveBasketAsync(basket, ct);
    }
}
