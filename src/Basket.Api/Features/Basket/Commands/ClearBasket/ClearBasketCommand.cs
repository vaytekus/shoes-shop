using MediatR;

namespace Basket.Api.Features.Basket.Commands.ClearBasket;

public record ClearBasketCommand(string CustomerId) : IRequest;
