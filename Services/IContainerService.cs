using Azure.Storage.Blobs.Models;
using AzureUdemy.Models.ViewModels;

namespace AzureUdemy.Services
{
    public interface IContainerService
    {
        ValueTask<List<BlobContainerItem>> GetAllContainers();
        Task<List<BlobContainerViewModel>> GetAllContainerAndBlobsAsync();
        Task<BlobContainerViewModel> GetAllBlobsForContainerAsync(string containerName);
        Task CreateContainer(string containerName);
        Task DeleteContainer(string containerName);
    }
}