using System.ComponentModel.DataAnnotations;

namespace AzureUdemy.Models.ViewModels;

public class BlobFileViewModel
{
    public BlobContainerViewModelItem Model { get; set; } = new();

    public IFormFile? File { get; set; } = null;

    [Required]
    public string ContainerName { get; set; } = string.Empty;
}
