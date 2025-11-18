using Azure.Storage.Blobs.Models;
using AzureUdemy.Models.ViewModels;

namespace AzureUdemy.Services
{
    public interface IContainerService
    {
        Task<List<BlobContainerViewModel>> GetAllContainerAndBlobs();
        Task<List<BlobContainerItem>> GetAllContainers();

        Task<BlobContainerViewModel> GetAllBlobsForContainer(string containerName);
        Task CreateContainer(string containerName);
        Task DeleteContainer(string containerName);
    }
}