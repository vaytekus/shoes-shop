using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;

namespace Catalog.Api.Infrastructure;

public class BlobStorageService(BlobServiceClient blobServiceClient, IConfiguration configuration)
{
    private readonly string _containerName = configuration["AzureStorage:ContainerName"]!;

    public async Task<string> UploadAsync(IFormFile file, CancellationToken ct)
    {
        var container = blobServiceClient.GetBlobContainerClient(_containerName);
        var blobName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName.ToString())}";
        var blob = container.GetBlobClient(blobName);

        await  blob.UploadAsync(file.OpenReadStream(), new BlobHttpHeaders
        {
            ContentType = file.ContentType
        }, cancellationToken: ct);

        return blob.Uri.ToString();
    }
}
