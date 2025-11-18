using Azure.Storage.Blobs.Models;

namespace AzureUdemy.Models.ViewModels;

public class ContainerViewModel
{
    public List<ContainerViewModelItem> Containers { get; init; } = [];

    public bool HasContainers => this.Containers.Count > 0;

    public static ContainerViewModel FromBlobContainerItems(IEnumerable<BlobContainerItem> blobContainerItems)
    {
        List<ContainerViewModelItem> containers = [.. blobContainerItems.Select(b => new ContainerViewModelItem { Name = b.Name })];

        return new ContainerViewModel { Containers = containers };
    }
}