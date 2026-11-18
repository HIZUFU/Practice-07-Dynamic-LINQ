using Microsoft.EntityFrameworkCore;
using ProductCatalogApp.Api.Data;
using ProductCatalogApp.Api.Models;
namespace ProductCatalogApp.Api.Jobs;

public class MonitoringJobs(ApplicationDbContext db, ILogger<MonitoringJobs> logger)
{
    public async Task CleanupReadings()
    {
        var cutoff = DateTime.UtcNow.AddDays(-7);
        var removed = await db.SensorReadings.Where(r => r.Timestamp < cutoff).ExecuteDeleteAsync();
        await db.JobExecutions.Where(j => j.CompletedAt < DateTime.UtcNow.AddDays(-30)).ExecuteDeleteAsync();
        await Save("Очистка показаний", $"Удалено замеров старше 7 дней: {removed}.");
    }

    public async Task InventoryReport(string taskName)
    {
        var sensors = await db.Sensors.CountAsync();
        var readings = await db.SensorReadings.CountAsync();
        var alarms = await db.SensorReadings.CountAsync(r => r.IsAlarm);
        await Save(taskName, $"Датчиков: {sensors}; замеров: {readings}; тревог: {alarms}.");
    }

    private async Task Save(string name, string summary)
    {
        db.JobExecutions.Add(new JobExecution { TaskName = name, CompletedAt = DateTime.UtcNow, Summary = summary });
        await db.SaveChangesAsync();
        logger.LogInformation("{Task}: {Summary}", name, summary);
    }
}
