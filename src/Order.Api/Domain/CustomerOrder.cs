namespace Order.Api.Domain;

public class CustomerOrder
{
    public Guid Id { get; set; }
    public string CustomerId { get; set; } = null!;
    public OrderStatus Status { get; set; } = OrderStatus.Pending;
    public ShippingAddress ShippingAddress { get; set; } = null!;
    public List<OrderItem> Items { get; set; } = [];
    public decimal TotalAmount { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
