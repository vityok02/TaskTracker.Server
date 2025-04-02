using Application.Abstract.Interfaces;
using Azure.Storage.Blobs;

namespace Infrastructure.BlobStorage;

public class BlobService : IBlobService
{
    private const string ContainerName = "avatars";

    private readonly BlobServiceClient _blobServiceClient;

    public BlobService(BlobServiceClient blobServiceClient)
    {
        _blobServiceClient = blobServiceClient;
    }

    public async Task<string> UploadAsync(
        string fileName,
        Stream content)
    {
        var containerClient = _blobServiceClient
            .GetBlobContainerClient(ContainerName);

        var blobClient = containerClient
            .GetBlobClient(fileName);

        await blobClient
            .UploadAsync(content, overwrite: true);

        return blobClient.Uri.AbsoluteUri;
    }
}
