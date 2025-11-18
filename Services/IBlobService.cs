using Azure.Storage.Blobs.Models;
using AzureUdemy.Models;
using AzureUdemy.Models.ViewModels;

namespace AzureUdemy.Services
{
    public interface IBlobService
    {
        Task<List<BlobItem>> GetAllBlobs(string containerName);
        Task<string> GetBlobUri(string name, string containerName);
        Task<bool> CreateBlob(string name, string containerName, IFormFile file, BlobContainerViewModelItem blobModel);
        Task<bool> DeleteBlob(string name, string containerName);
    }
}