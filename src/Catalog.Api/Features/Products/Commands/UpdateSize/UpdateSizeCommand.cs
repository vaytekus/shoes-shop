using MediatR;

namespace Catalog.Api.Features.Products.Commands.UpdateSize;

public record UpdateSizeCommand(
    Guid SizeId,
    string Size,
    int StockQuantity) : IRequest;
