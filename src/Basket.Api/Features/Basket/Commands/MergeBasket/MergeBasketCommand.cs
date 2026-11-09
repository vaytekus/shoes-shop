using Basket.Api.Domain;
using MediatR;

namespace Basket.Api.Features.Basket.Commands.MergeBasket;

public record MergeBasketCommand(string CustomerId, Guid GuestId) : IRequest<CustomerBasket>;
