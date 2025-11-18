using System.ComponentModel.DataAnnotations;

namespace AzureUdemy.Models.ViewModels;

public record ContainerViewModelItem
{
    [Required]
    public string Name { get; init; } = string.Empty;
}