namespace Catalog.Api.DTOs.StockCheck;

public record OutOfStockItem(Guid ProductId, string Size, int Available, int Requested);
