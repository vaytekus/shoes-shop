namespace Catalog.Api.DTOs.StockCheck;

public record StockCheckItem(Guid ProductId, string Size, int Quantity);
