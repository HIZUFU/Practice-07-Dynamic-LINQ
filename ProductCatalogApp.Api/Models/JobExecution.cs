namespace ProductCatalogApp.Api.Models;

public class JobExecution
{
    public int Id { get; set; }
    public string TaskName { get; set; } = string.Empty;
    public DateTime CompletedAt { get; set; }
    public string Summary { get; set; } = string.Empty;
}
