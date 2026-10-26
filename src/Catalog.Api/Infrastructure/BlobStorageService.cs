using System.Text.RegularExpressions;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;

namespace Catalog.Api.Infrastructure;

public class BlobStorageService(BlobServiceClient blobServiceClient, IConfiguration configuration)
{
    private readonly string _containerName = configuration["AzureStorage:ContainerName"]!;

    public async Task<string> UploadAsync(
        IFormFile file,
        string section,
        string category,
        string brand,
        string modelName,
        CancellationToken ct)
    {
        var container = blobServiceClient.GetBlobContainerClient(_containerName);
        var now = DateTime.UtcNow;
        var blobName = $"{now:yyyy}/{now:MM}/{Slugify(section)}/{Slugify(category)}/{Slugify(brand)}-{Slugify(modelName)}/{Guid.NewGuid
            ()}{Path.GetExtension(file.FileName)}";
        var blob = container.GetBlobClient(blobName);

        await  blob.UploadAsync(file.OpenReadStream(), new BlobHttpHeaders
        {
            ContentType = file.ContentType
        }, cancellationToken: ct);

        return blob.Uri.ToString();
    }

    public async Task DeleteAsync(string url, CancellationToken ct = default)
    {
        var containerName = configuration["AzureStorage:ContainerName"]!;
        var container = blobServiceClient.GetBlobContainerClient(containerName);
        var uri = new Uri(url);
        var blobName = uri.AbsolutePath.TrimStart('/').Substring(containerName.Length + 1);
        await container.GetBlobClient(blobName).DeleteIfExistsAsync(cancellationToken: ct);
    }

    private static string Slugify(string value)
    {
        return Regex.Replace(value.ToLowerInvariant().Trim(), @"[^a-z0-9]+", "-").Trim('-');
    }
}
