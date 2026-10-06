namespace Catalog.Api.Features.Products.Commands.UploadProductImages;

public class UploadProductImagesValidator
{
    private static readonly string[] AllowedTypes = ["image/jpeg", "image/png", "image/webp"];
    private const long MaxFileSize = 5 * 1024 * 1024;

    public (bool isValid, string? Error) Validate(IFormFileCollection files)
    {
        if (files.Count == 0)
        {
            return (false, "At least one image is required");
        }

        foreach (var file in files)
        {
            if (!AllowedTypes.Contains(file.ContentType))
            {
                return (false, $"{file.FileName}: only JPEG, PNG and WebP are allowed");
            }

            if (file.Length > MaxFileSize)
            {
                return (false, $"{file.FileName}: file size must not exceed 5MB");
            }
        }

        return (true, null);
    }
}
