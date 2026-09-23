using MediatR;

namespace Basket.Api.Features.Basket.Commands.RemoveItem;

public record RemoveItemCommand(
    string CustomerId,
    Guid ProductId
) : IRequest;
