using Azure;
using Azure.Storage.Blobs.Models;
using AzureUdemy.Models.ViewModels;

namespace AzureUdemy.Services
{
    public interface IBlobService
    {
        ValueTask<List<BlobItem>> GetAllBlobs(string containerName);
        string GetBlobAbsoluteUri(string name, string containerName);
        Task<bool> CreateBlob(string name, string containerName, IFormFile file, BlobContainerViewModelItem blobModel);
        Task<Response<bool>> DeleteBlob(string name, string containerName);
    }
}