using Basket.Api.Domain;
using MediatR;

namespace Basket.Api.Features.Basket.Queries.GetBasket;

public record GetBasketQuery(string CustomerId) : IRequest<CustomerBasket?>;
