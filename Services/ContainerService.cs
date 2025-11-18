using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using AzureUdemy.Models.ViewModels;

namespace AzureUdemy.Services;

public class ContainerService(BlobServiceClient blobServiceClient) : BaseBlobService(blobServiceClient), IContainerService
{
    public async Task CreateContainer(string containerName) =>
        await this.GetContainerClient(containerName).CreateIfNotExistsAsync(PublicAccessType.BlobContainer);

    public async Task DeleteContainer(string containerName) =>
        await this.GetContainerClient(containerName).DeleteIfExistsAsync();
    public async Task<List<BlobContainerItem>> GetAllContainers() =>
        await this._blobClient.GetBlobContainersAsync().ToListAsync();

    public async Task<List<BlobContainerViewModel>> GetAllContainerAndBlobs()
    {
        List<BlobContainerViewModel> result = [];
        List<BlobContainerItem> containerItems = await this.GetAllContainers();

        foreach (BlobContainerItem containerItem in containerItems)
        {
            List<BlobContainerViewModelItem> blobs = await this.GetContainerBlobsModels(containerItem.Name);
            result.Add(new BlobContainerViewModel
            {
                ContainerName = containerItem.Name,
                Blobs = blobs
            });
        }

        return result;
    }
    public async Task<BlobContainerViewModel> GetAllBlobsForContainer(string containerName) => new()
    {
        ContainerName = containerName,
        Blobs = await this.GetContainerBlobsModels(containerName)
    };
}