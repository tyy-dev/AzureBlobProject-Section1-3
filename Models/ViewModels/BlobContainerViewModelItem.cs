namespace AzureUdemy.Models.ViewModels;

public class BlobContainerViewModelItem
{
    public string Name { get; set; } = string.Empty;
    public string? Title { get; set; }
    public string? Comment { get; set; }
    public string? Uri { get; set; }
}