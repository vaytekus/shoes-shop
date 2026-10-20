using Catalog.Api.Infrastructure;
using MediatR;

namespace Catalog.Api.Features.Products.Commands.UploadProductImages;

public static class UploadProductImagesEndpoint
{
    public static async Task<IResult> Handle(
        Guid id,
        Guid variantId,
        IFormFileCollection files,
        string? gender,
        string? category,
        string? brand,
        string? modelName,
        BlobStorageService blobService,
        UploadProductImagesValidator validator,
        ISender sender,
        CancellationToken ct)
    {
        var (isValid, error) = validator.Validate(files);
        if (!isValid)
        {
            return Results.BadRequest(error);
        }

        var url = await Task.WhenAll(files.Select(f => blobService.UploadAsync(f,
            gender ?? "other",
            category ?? "other",
            brand ?? "unknown",
            modelName ?? id.ToString(),
            ct)));
        var success = await sender.Send(new UploadProductImagesCommand(variantId, url), ct);

        if (!success)
        {
            return Results.NotFound();
        }

        return Results.Ok(new { url });
    }
}
