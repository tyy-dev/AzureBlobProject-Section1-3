using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using AzureUdemy.Models.ViewModels;

namespace AzureUdemy.Services;

public class ContainerService(BlobServiceClient blobServiceClient) : BaseBlobService(blobServiceClient), IContainerService
{
    public ValueTask<List<BlobContainerItem>> GetAllContainers() =>
        this._blobClient.GetBlobContainersAsync().ToListAsync();
    public async Task<List<BlobContainerViewModel>> GetAllContainerAndBlobsAsync()
    {
        List<BlobContainerViewModel> result = [];
        List<BlobContainerItem> containerItems = await this.GetAllContainers().ConfigureAwait(false);

        foreach (BlobContainerItem containerItem in containerItems)
        {
            List<BlobContainerViewModelItem> blobs = await this.GetContainerBlobsModels(containerItem.Name).ConfigureAwait(false);
            result.Add(new BlobContainerViewModel
            {
                ContainerName = containerItem.Name,
                Blobs = blobs,
            });
        }

        return result;
    }
    public async Task<BlobContainerViewModel> GetAllBlobsForContainerAsync(string containerName) => new()
    {
        ContainerName = containerName,
        Blobs = await this.GetContainerBlobsModels(containerName).ConfigureAwait(false),
    };

    public Task CreateContainer(string containerName) =>
        this.GetContainerClient(containerName).CreateIfNotExistsAsync(PublicAccessType.BlobContainer);

    public Task DeleteContainer(string containerName) =>
        this.GetContainerClient(containerName).DeleteIfExistsAsync();
}