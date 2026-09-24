namespace Order.Api.DTOs;

public record ShippingAddressDto(
    string Street,
    string City,
    string Country,
    string ZipCode);
