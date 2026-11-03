using MediatR;
using Wishlist.Api.Domain;

namespace Wishlist.Api.Features.Wishlist.Queries.GetWishlist;

public record GetWishlistQuery(string CustomerId) : IRequest<CustomerWishlist>;
