namespace Order.Api.Infrastructure;

public class CatalogClient(HttpClient http)
{
    public async Task<StockCheckResult?> CheckStockAsync(
        List<StockCheckItem> items,
        CancellationToken ct)
    {
        var response = await http.PostAsJsonAsync("/api/products/stock/check", items, ct);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<StockCheckResult>(ct);
    }
}

public record StockCheckItem(Guid ProductId, string Size, int Quantity);

public record StockCheckResult(bool IsAvailable, List<OutOfStockItem> UnavailableItems);
public record OutOfStockItem(Guid ProductId, string Size, int Available, int Requested);