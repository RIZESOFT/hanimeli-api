using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;

namespace HanimeliApp.Application.Utilities;

public class AzureStorageHelper
{
    private readonly BlobServiceClient _blobServiceClient;

    public AzureStorageHelper(BlobServiceClient blobServiceClient)
    {
        _blobServiceClient = blobServiceClient;
    }

    private async Task<BlobContainerClient> GetOrCreateContainerAsync(string containerName)
    {
        var blobContainerClient = _blobServiceClient.GetBlobContainerClient(containerName);
        await blobContainerClient.CreateIfNotExistsAsync(PublicAccessType.Blob);
        return blobContainerClient;
    }

    private async Task<BlobClient> GetBlobAsync(string containerName, string fileName)
    {
        var blobContainerClient = await GetOrCreateContainerAsync(containerName);
        var blobClient = blobContainerClient.GetBlobClient(fileName);
        return blobClient;
    }
    
    private async Task<string> GetStoragePathAsync(string storageContainer)
    {
        var container = await GetOrCreateContainerAsync(storageContainer);

        return container.Uri.AbsoluteUri;
    }
    
    public async Task<Stream> GetFileAsync(string containerName, string fileName)
    {
        var blobClient = await GetBlobAsync(containerName, fileName.ToLower());
        return await blobClient.OpenReadAsync();
    }
    
    public async Task UploadFileAsync(string containerName, string fileName, Stream fileStream, string contentType)
    {
        var blobClient = await GetBlobAsync(containerName, fileName.ToLower());
        var blobHttpHeaders = new BlobHttpHeaders { ContentType = contentType };
        var uploadOptions = new BlobUploadOptions
        {
            HttpHeaders = blobHttpHeaders
        };
        await blobClient.UploadAsync(fileStream, uploadOptions);
    }
    
    public async Task DeleteFileAsync(string storageContainer, string fileName)
    {
        var blobClient = await GetBlobAsync(storageContainer, fileName.ToLower());
        await blobClient.DeleteIfExistsAsync();
    }
    
    public async Task<string> GetFilePathAsync(string storageContainer, string fileName)
    {
        return new Uri(Path.Combine(await GetStoragePathAsync(storageContainer), fileName)).AbsoluteUri;
    }
}