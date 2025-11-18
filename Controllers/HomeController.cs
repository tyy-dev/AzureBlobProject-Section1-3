using AzureUdemy.Models.ViewModels;
using AzureUdemy.Services;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace AzureUdemy.Controllers
{
    public class HomeController(IContainerService containerService) : Controller
    {
        private readonly IContainerService _containerService = containerService;

        public async Task<IActionResult> Index()
        {
            List<BlobContainerViewModel> containers = await this._containerService.GetAllContainerAndBlobs();
            HomeViewModel viewModel = new() { Containers = containers };
            return this.View(viewModel);
        }

        public async Task<IActionResult> SingleContainer(string containerName)
        {
            BlobContainerViewModel blobs = await this._containerService.GetAllBlobsForContainer(containerName);
            return this.View("Index", new HomeViewModel
            {
                Containers = [blobs]
            });
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error() => this.View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? this.HttpContext.TraceIdentifier });
    }
}
