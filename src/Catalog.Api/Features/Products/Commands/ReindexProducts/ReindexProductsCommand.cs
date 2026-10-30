using MediatR;

namespace Catalog.Api.Features.Products.Commands.ReindexProducts;

public record ReindexProductsCommand : IRequest<int>;
