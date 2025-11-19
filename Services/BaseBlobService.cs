using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Blobs.Specialized;
using Azure.Storage.Sas;
using AzureUdemy.Models.ViewModels;

namespace AzureUdemy.Services;

public abstract class BaseBlobService(BlobServiceClient blobServiceClient) {
    protected readonly BlobServiceClient _blobClient = blobServiceClient;

    protected BlobContainerClient GetContainerClient(string containerName)
        => this._blobClient.GetBlobContainerClient(containerName);
    protected BlobClient GetBlobClient(string name, string containerName) =>
        this.GetContainerClient(containerName).GetBlobClient(name);

    protected Uri GenerateSasUri(object client, BlobSasPermissions permissions = BlobSasPermissions.Read, TimeSpan? expiry = null)
    {
        expiry ??= TimeSpan.FromHours(1);

        BlobSasBuilder sasBuilder = new()
        {
            ExpiresOn = DateTimeOffset.UtcNow.Add(expiry.Value),
        };
        sasBuilder.SetPermissions(permissions);

        switch (client)
        {
            case BlobClient blobClient:
                if (!blobClient.CanGenerateSasUri)
                {
                    return blobClient.Uri;
                }

                sasBuilder.BlobContainerName = blobClient.GetParentBlobContainerClient().Name;
                sasBuilder.BlobName = blobClient.Name;
                sasBuilder.Resource = "b";

                return blobClient.GenerateSasUri(sasBuilder);

            case BlobContainerClient containerClient:
                if (!containerClient.CanGenerateSasUri)
                {
                    return containerClient.Uri;
                }

                sasBuilder.BlobContainerName = containerClient.Name;
                sasBuilder.Resource = "c";

                return containerClient.GenerateSasUri(sasBuilder);

            default:
                throw new ArgumentException("Client must be either BlobClient or BlobContainerClient", nameof(client));
        }
    }

    protected async Task<List<BlobContainerViewModelItem>> GetContainerBlobsModels(string containerName)
    {
        BlobContainerClient blobContainerClient = this.GetContainerClient(containerName);

        string sasContainerSignature = "";
        if (blobContainerClient.CanGenerateSasUri)
        {
            sasContainerSignature = this.GenerateSasUri(blobContainerClient).Query;
        }

        List<BlobContainerViewModelItem> blobs = [];
        await foreach (BlobItem blobItem in blobContainerClient.GetBlobsAsync().ConfigureAwait(false))
        {
            BlobClient blobClient = this.GetBlobClient(blobItem.Name, containerName);

            BlobProperties properties = await blobClient.GetPropertiesAsync().ConfigureAwait(false);

            // Make sure the sasContainerQuery is appended correctly in the case the blob Uri already has query parameters
            Uri uri = blobClient.Uri;
            string absoluteUri = uri.AbsoluteUri;
            if (!string.IsNullOrEmpty(uri.Query))
            {
                absoluteUri += sasContainerSignature.TrimStart('?') + "&";
            }
            else
            {
                absoluteUri += sasContainerSignature;
            }

            BlobContainerViewModelItem blobModel = new()
            {
                Name = blobItem.Name,
                Title = properties.Metadata.TryGetValue("title", out string? title) ? title : null,
                Comment = properties.Metadata.TryGetValue("comment", out string? comment) ? comment : null,
                Uri = absoluteUri,
            };


            blobModel.Uri = this.GenerateSasUri(blobClient).AbsoluteUri;

            blobs.Add(blobModel);
        }

        return blobs;
    }
}
