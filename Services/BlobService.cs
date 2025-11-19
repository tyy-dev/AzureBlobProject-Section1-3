using Azure;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using AzureUdemy.Models.ViewModels;

namespace AzureUdemy.Services;

public class BlobService(BlobServiceClient blobServiceClient) : BaseBlobService(blobServiceClient), IBlobService
{
    public ValueTask<List<BlobItem>> GetAllBlobs(string containerName) =>
        this.GetContainerClient(containerName).GetBlobsAsync().ToListAsync();
    public string GetBlobAbsoluteUri(string name, string containerName) =>
       this.GenerateSasUri(this.GetBlobClient(name, containerName)).AbsoluteUri;

    public async Task<bool> CreateBlob(string name, string containerName, IFormFile file, BlobContainerViewModelItem blobModel)
    {
        BlobClient blobClient = this.GetBlobClient(name, containerName);

        BlobHttpHeaders httpHeaders = new()
        {
            ContentType = file.ContentType,
        };

        Dictionary<string, string> metaData = [];

        if (!string.IsNullOrEmpty(blobModel.Title))
        {
            metaData.Add("title", blobModel.Title);
        }

        if (!string.IsNullOrEmpty(blobModel.Comment))
        {
            metaData.Add("comment", blobModel.Comment);
        }

        Response<BlobContentInfo> result = await blobClient
            .UploadAsync(file.OpenReadStream(), httpHeaders, metaData)
            .ConfigureAwait(false);

        return result != null;
    }

    public Task<Response<bool>> DeleteBlob(string name, string containerName) => this.GetBlobClient(name, containerName).DeleteIfExistsAsync();
}