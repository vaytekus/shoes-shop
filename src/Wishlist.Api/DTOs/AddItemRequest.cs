using System;

namespace Wishlist.Api.DTOs;

public record AddItemRequest(Guid ProductId, string ProductName, decimal Price, string ImageUrl, string Section, string CategoryName);
