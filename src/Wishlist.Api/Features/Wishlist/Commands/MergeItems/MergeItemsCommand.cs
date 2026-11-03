using System.Collections.Generic;
using MediatR;
using Wishlist.Api.Domain;
using Wishlist.Api.DTOs;

namespace Wishlist.Api.Features.Wishlist.Commands.MergeItems;

public record MergeItemsCommand(string CustomerId, List<AddItemRequest> Items) : IRequest<CustomerWishlist>;
