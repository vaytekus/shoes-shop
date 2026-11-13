namespace Order.Api.Domain;

public class CustomerOrder
{
    public Guid Id { get; set; }
    public string CustomerId { get; set; } = null!;
    public OrderStatus Status { get; set; } = OrderStatus.Pending;

    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string Phone { get; set; } = null!;
    public string Email { get; set; } = null!;

    public DeliveryType DeliveryType { get; set; }
    public PaymentMethod PaymentMethod { get; set; }

    public ShippingAddress ShippingAddress { get; set; } = null!;
    public List<OrderItem> Items { get; set; } = [];
    public decimal TotalAmount { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
