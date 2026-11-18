using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductCatalogApp.Client.Models;
namespace ProductCatalogApp.Client.Controllers;

[Authorize(Roles = "Admin")]
public class JobsController(IHttpClientFactory factory) : Controller
{
    private HttpClient Client() => factory.CreateClient("CatalogApi");

    public async Task<IActionResult> Index()
    {
        using var client = Client();
        return View(await client.GetFromJsonAsync<JobsViewModel>("api/jobs") ?? new JobsViewModel());
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Run(string kind)
    {
        if (kind is not ("run-once" or "delayed" or "cleanup")) return BadRequest();
        using var client = Client();
        using var response = await client.PostAsync("api/jobs/" + kind, null);
        response.EnsureSuccessStatusCode();
        TempData["Message"] = "Задача поставлена в очередь. Обновите страницу, чтобы увидеть результат.";
        return RedirectToAction(nameof(Index));
    }
}
