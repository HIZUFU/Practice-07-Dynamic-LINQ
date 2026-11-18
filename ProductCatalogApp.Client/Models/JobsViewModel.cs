namespace ProductCatalogApp.Client.Models;

public class JobsViewModel
{
    public List<ScheduleItem> Schedules { get; set; } = new();
    public List<ScheduledItem> Scheduled { get; set; } = new();
    public List<JobLogItem> Logs { get; set; } = new();
}
public record ScheduleItem(string Id, string Cron, DateTime? NextExecution, DateTime? LastExecution);
public record ScheduledItem(string Id, DateTime EnqueueAt);
public record JobLogItem(int Id, string TaskName, DateTime CompletedAt, string Summary);
