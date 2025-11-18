using Azure.Storage.Blobs.Models;
using AzureUdemy.Models.ViewModels;
using AzureUdemy.Services;
using Microsoft.AspNetCore.Mvc;

namespace AzureUdemy.Controllers;

public class ContainerController(IContainerService containerService) : Controller
{
    private readonly IContainerService _containerService = containerService;

    public async Task<IActionResult> Index()
    {
        List<BlobContainerItem> containerItems = await this._containerService.GetAllContainers();

        ContainerViewModel viewModel = ContainerViewModel.FromBlobContainerItems(containerItems);
        return this.View(viewModel);
    }

    public async Task<IActionResult> Create() => this.View(new ContainerViewModelItem());

    [HttpPost]
    public async Task<IActionResult> Create(ContainerViewModelItem container)
    {
        await this._containerService.CreateContainer(container.Name);
        return this.RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Delete(string containerName)
    {
        await this._containerService.DeleteContainer(containerName);
        return this.RedirectToAction(nameof(Index));
    }
}
