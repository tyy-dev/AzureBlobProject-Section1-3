using Azure.Storage.Blobs.Models;

namespace AzureUdemy.Models.ViewModels;

public record BlobContainerViewModel
{
    public required string ContainerName { get; init; }

    public List<BlobContainerViewModelItem> Blobs { get; init; } = [];

    public bool HasBlobs => this.Blobs.Count > 0;
    public static BlobContainerViewModel FromBlobItems(string containerName, IEnumerable<BlobItem> blobItems)
    {
        List<BlobContainerViewModelItem> blobs = [.. blobItems.Select(b => new BlobContainerViewModelItem { Name = b.Name })];

        return new BlobContainerViewModel
        {
            ContainerName = containerName,
            Blobs = blobs,
        };
    }

}
