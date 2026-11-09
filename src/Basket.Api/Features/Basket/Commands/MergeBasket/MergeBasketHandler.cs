using Basket.Api.Domain;
using Basket.Api.Infrastructure;
using MediatR;

namespace Basket.Api.Features.Basket.Commands.MergeBasket;

public class MergeBasketHandler(IBasketRepository repository) : IRequestHandler<MergeBasketCommand, CustomerBasket>
{
    public async Task<CustomerBasket> Handle(MergeBasketCommand request, CancellationToken ct)
    {
        var guestId = request.GuestId.ToString();
        var guestBasket = await repository.GetBasketAsync(guestId, ct);

        if (guestBasket is null || guestBasket.Items.Count == 0)
        {
            if (guestBasket is not null)
            {
                await repository.DeleteBasketAsync(guestId, ct);
            }

            return await repository.GetBasketAsync(request.CustomerId, ct)
                ?? new CustomerBasket{ CustomerId = request.CustomerId };
        }

        var basket = await repository.GetBasketAsync(request.CustomerId, ct)
            ?? new CustomerBasket
            {
                CustomerId = request.CustomerId
            };

        foreach (var guestIstem in guestBasket.Items)
        {
            var existing = basket.Items.FirstOrDefault(i => i.ProductId == guestIstem.ProductId);
            if (existing is not null)
            {
                existing.Quantity += guestIstem.Quantity;
            }
            else
            {
                basket.Items.Add(new BasketItem
                {
                    ProductId = guestIstem.ProductId,
                    ProductName = guestIstem.ProductName,
                    Price =  guestIstem.Price,
                    Quantity = guestIstem.Quantity,
                    ImageUrl =  guestIstem.ImageUrl,
                    CategoryName = guestIstem.CategoryName
                });
            }
        }

        await repository.SaveBasketAsync(basket, ct);
        await repository.DeleteBasketAsync(guestId, ct);

        return basket;
    }
}
