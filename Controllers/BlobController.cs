using Azure.Storage.Blobs.Models;
using AzureUdemy.Models.ViewModels;
using AzureUdemy.Services;
using Microsoft.AspNetCore.Mvc;

namespace AzureUdemy.Controllers;

public class BlobController(IBlobService blobService) : Controller
{
    private readonly IBlobService _blobService = blobService;

    [HttpGet]
    public async Task<IActionResult> Manage(string containerName)
    {
        List<BlobItem> blobItems = await this._blobService.GetAllBlobs(containerName);
        BlobContainerViewModel viewModel = BlobContainerViewModel.FromBlobItems(containerName, blobItems);
        return this.View(viewModel);
    }

    [HttpGet]
    public IActionResult AddFile(string containerName)
    {
        BlobFileViewModel model = new()
        {
            ContainerName = containerName
        };
        return this.View(model);
    }

    [HttpPost]
    public async Task<IActionResult> AddFile(BlobFileViewModel blobFileModel)
    {
        if (!this.ModelState.IsValid || blobFileModel.File?.Length == 0)
        {
            this.ModelState.AddModelError("", "Please provide a file to upload.");
            return this.View(blobFileModel);
        }

        IFormFile file = blobFileModel.File!;

        string fileName = Path.GetFileNameWithoutExtension(file.FileName) + "_" + Guid.NewGuid() + Path.GetExtension(file.FileName);

        bool result = await this._blobService.CreateBlob(
            fileName,
            blobFileModel.ContainerName,
            file,
            blobFileModel.Model
        );

        if (!result)
        {
            this.ModelState.AddModelError("", "Failed to upload the blob. Please try again.");
            return this.View(blobFileModel);
        }

        return this.RedirectToAction("Manage", new { containerName = blobFileModel.ContainerName });
    }

    [HttpGet]
    public async Task<IActionResult> ViewFile(string name, string containerName) =>
        this.Redirect(await this._blobService.GetBlobUri(name, containerName));

    public async Task<IActionResult> DeleteFile(string name, string containerName)
    {
        await this._blobService.DeleteBlob(name, containerName);
        return this.RedirectToAction("Manage", new { containerName });
    }
}