namespace Order.Api.Domain;

public class ShippingAddress
{
    public string Street { get; set; } = null!;
    public string City { get; set; } = null!;
    public string Country { get; set; } = null!;
    public string ZipCode { get; set; } = null!;
}
