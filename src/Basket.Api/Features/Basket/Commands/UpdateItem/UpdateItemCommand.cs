using Basket.Api.Domain;
using MediatR;

namespace Basket.Api.Features.Basket.Commands.UpdateItem;

public record UpdateItemCommand(
    string CustomerId,
    Guid ProductId,
    string ProductName,
    decimal Price,
    int Quantity,
    string ImageUrl) : IRequest<CustomerBasket>;
