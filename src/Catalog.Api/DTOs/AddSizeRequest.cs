namespace Catalog.Api.DTOs;

public record AddSizeRequest(
    string Size,
    int StockQuantity);
