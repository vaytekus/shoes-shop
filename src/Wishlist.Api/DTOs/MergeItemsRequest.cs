using System.Collections.Generic;

namespace Wishlist.Api.DTOs;

public record MergeItemsRequest(List<AddItemRequest> Items);
