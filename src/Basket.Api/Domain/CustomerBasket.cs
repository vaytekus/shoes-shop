namespace Basket.Api.Domain;

public class CustomerBasket
{
    public string CustomerId { get; set; } = null!;
    public List<BasketItem> Items { get; set; } = [];

    public decimal TotalPrice => Items.Sum(item => item.Price * item.Quantity);
}
