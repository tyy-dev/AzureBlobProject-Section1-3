namespace AzureUdemy.Models.ViewModels
{
    public class HomeViewModel
    {
        public List<BlobContainerViewModel> Containers { get; init; } = [];

        public bool HasContainers => this.Containers.Count > 0;
    }
}
