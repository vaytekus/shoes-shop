using System;
using MediatR;
using Wishlist.Api.Domain;

namespace Wishlist.Api.Features.Wishlist.Commands.AddItem;

public record AddItemCommand(
    string CustomerId,
    Guid ProductId,
    string ProductName,
    decimal Price,
    string ImageUrl,
    string Section,
    string CategoryName) : IRequest<CustomerWishlist>;
