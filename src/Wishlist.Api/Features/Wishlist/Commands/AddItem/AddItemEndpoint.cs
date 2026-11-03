using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using Wishlist.Api.DTOs;

namespace Wishlist.Api.Features.Wishlist.Commands.AddItem;

public static class AddItemEndpoint
{
    public static async Task<IResult> Handle(
        IMediator mediator,
        string customerId,
        AddItemRequest request,
        CancellationToken ct)
    {
        var command = new AddItemCommand(
        customerId,
        request.ProductId,
        request.ProductName,
        request.Price,
        request.ImageUrl,
        request.Section,
        request.CategoryName);

        var result = await mediator.Send(command, ct);
        return Results.Ok(result);
    }
}
