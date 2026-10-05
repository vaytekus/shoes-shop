using Catalog.Api.DTOs;
using MediatR;

namespace Catalog.Api.Features.Products.Commands.UpdateProduct;

public record UpdateProductCommand(
    Guid Id,
    string Name,
    string Description,
    decimal Price,
    string ImageUrl,
    int StockQuantity,
    Guid? CategoryId) : IRequest;
