using Azure;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Blobs.Specialized;
using Azure.Storage.Sas;
using AzureUdemy.Models.ViewModels;

namespace AzureUdemy.Services;

public abstract class BaseBlobService(BlobServiceClient blobServiceClient)
{
    protected readonly BlobServiceClient _blobClient = blobServiceClient;

    protected BlobContainerClient GetContainerClient(string containerName)
        => this._blobClient.GetBlobContainerClient(containerName);
    protected BlobClient GetBlobClient(string name, string containerName) =>
        this.GetContainerClient(containerName).GetBlobClient(name);

    protected async Task<List<BlobContainerViewModelItem>> GetContainerBlobsModels(string containerName)
    {
        BlobContainerClient blobContainerClient = this.GetContainerClient(containerName);

        string sasContainerSignature = "";
        if (blobContainerClient.CanGenerateSasUri)
        {
            BlobSasBuilder blobSasBuilder = new()
            {
                BlobContainerName = blobContainerClient.Name,
                Resource = "c",
                ExpiresOn = DateTimeOffset.UtcNow.AddHours(1)
            };

            blobSasBuilder.SetPermissions(BlobSasPermissions.Read);

            sasContainerSignature = blobContainerClient.GenerateSasUri(blobSasBuilder).AbsoluteUri.Split('?')[1];
        }


        List<BlobContainerViewModelItem> blobs = [];
        await foreach (BlobItem blobItem in blobContainerClient.GetBlobsAsync())
        {
            BlobClient blobClient = this.GetBlobClient(blobItem.Name, containerName);

            BlobProperties properties = await blobClient.GetPropertiesAsync();
            BlobContainerViewModelItem blobModel = new()
            {
                Name = blobItem.Name,
                Title = properties.Metadata.TryGetValue("title", out string? title) ? title : null,
                Comment = properties.Metadata.TryGetValue("comment", out string? comment) ? comment : null,
                Uri = blobClient.Uri.AbsoluteUri
            };

            if (blobClient.CanGenerateSasUri)
            {
                BlobSasBuilder blobSasBuilder = new()
                {
                    BlobContainerName = blobClient.GetParentBlobContainerClient().Name,
                    BlobName = blobClient.Name,
                    Resource = "b",
                    ExpiresOn = DateTimeOffset.UtcNow.AddHours(1)
                };

                blobSasBuilder.SetPermissions(BlobSasPermissions.Read);

                blobModel.Uri = blobClient.GenerateSasUri(blobSasBuilder).AbsoluteUri;
            }

            blobs.Add(blobModel);
        }

        return blobs;
    }

}