using Azure;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using AzureUdemy.Models.ViewModels;

namespace AzureUdemy.Services;

public class BlobService(BlobServiceClient blobServiceClient) : BaseBlobService(blobServiceClient), IBlobService
{
    public async Task<bool> CreateBlob(string name, string containerName, IFormFile file, BlobContainerViewModelItem blobModel)
    {
        BlobClient blobClient = this.GetBlobClient(name, containerName);

        BlobHttpHeaders httpHeaders = new()
        {
            ContentType = file.ContentType
        };

        Dictionary<string, string> metaData = [];

        if (!string.IsNullOrEmpty(blobModel.Title))
            metaData.Add("title", blobModel.Title);
        if (!string.IsNullOrEmpty(blobModel.Comment))
            metaData.Add("comment", blobModel.Comment);

        Response<BlobContentInfo> result = await blobClient.UploadAsync(file.OpenReadStream(), httpHeaders, metaData);

        return result != null;
    }

    public async Task<bool> DeleteBlob(string name, string containerName)
    {
        BlobClient blobClient = this.GetBlobClient(name, containerName);
        return await blobClient.DeleteIfExistsAsync();
    }

    public async Task<List<BlobItem>> GetAllBlobs(string containerName) =>
        await this.GetContainerClient(containerName).GetBlobsAsync().ToListAsync();

    public async Task<string> GetBlobUri(string name, string containerName) =>
        this.GetContainerClient(containerName).GetBlobClient(name)?.Uri.AbsoluteUri ?? string.Empty;
}