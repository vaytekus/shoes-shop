namespace Basket.Api.Domain;

public class BasketItem
{
    public Guid ProductId { get; set; }
    public string ProductName { get; set; } = null!;
    public decimal Price { get; set; }
    public int Quantity { get; set; }
    public string ImageUrl { get; set; } = null!;
}
