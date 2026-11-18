using Microsoft.AspNetCore.Authorization;
using Hangfire;
using Hangfire.Storage;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductCatalogApp.Api.Data;
using ProductCatalogApp.Api.Jobs;
namespace ProductCatalogApp.Api.Controllers;

[Authorize(Roles = "Admin")]
[ApiController, Route("api/jobs")]
public class JobsController(ApplicationDbContext db, IBackgroundJobClient jobs, IRecurringJobManager recurring, JobStorage storage) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        using var connection = storage.GetConnection();
        var schedules = connection.GetRecurringJobs().Select(j => new { j.Id, j.Cron, j.NextExecution, j.LastExecution });
        var monitoring = storage.GetMonitoringApi();
        var scheduled = monitoring.ScheduledJobs(0, 20).Select(j => new { Id = j.Key, j.Value.EnqueueAt });
        var logs = await db.JobExecutions.AsNoTracking().OrderByDescending(j => j.Id).Take(50).ToListAsync();
        return Ok(new { schedules, scheduled, logs });
    }

    [HttpPost("run-once")]
    public IActionResult RunOnce() => Accepted(new { id = jobs.Enqueue<MonitoringJobs>(job => job.InventoryReport("Разовая сводка")) });

    [HttpPost("delayed")]
    public IActionResult Delayed() => Accepted(new { id = jobs.Schedule<MonitoringJobs>(job => job.InventoryReport("Отложенная сводка"), TimeSpan.FromSeconds(15)) });

    [HttpPost("cleanup")]
    public IActionResult Cleanup()
    {
        recurring.Trigger("cleanup-readings");
        return Accepted();
    }
}
