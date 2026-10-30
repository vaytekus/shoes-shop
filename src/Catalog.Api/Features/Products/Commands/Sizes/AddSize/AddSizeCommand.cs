using MediatR;

namespace Catalog.Api.Features.Products.Commands.Sizes.AddSize;

public record AddSizeCommand(
    Guid VariantId,
    string Size,
    int StockQuantity) : IRequest<Guid>;
