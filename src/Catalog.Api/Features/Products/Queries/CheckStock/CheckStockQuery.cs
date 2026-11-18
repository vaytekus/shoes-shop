using Catalog.Api.DTOs.StockCheck;
using MediatR;

namespace Catalog.Api.Features.Products.Queries.CheckStock;

public record CheckStockQuery(List<StockCheckItem> Items) : IRequest<StockCheckResult>;
