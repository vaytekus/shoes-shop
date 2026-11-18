namespace Catalog.Api.DTOs.StockCheck;

public record StockCheckResult(bool IsAvailable, List<OutOfStockItem> UnavailableItems);
