namespace ShoesShop.Shared.Events;

public record OrderCreatedEvent(Guid OrderId, string CustomerId);
