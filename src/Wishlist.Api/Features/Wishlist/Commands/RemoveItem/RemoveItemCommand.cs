using System;
using MediatR;

namespace Wishlist.Api.Features.Wishlist.Commands;

public record RemoveItemCommand(string CustomerId, Guid ProductId) : IRequest;
