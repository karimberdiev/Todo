using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Todo.AuthMVC.Models;
using Todo.AuthMVC.Services;
using Todo.AuthMVC.ViewModels;

namespace Todo.AuthMVC.Controllers;

[Authorize]
public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly ApiClient _apiClient;

    public HomeController(ILogger<HomeController> logger, ApiClient apiClient)
    {
        _logger = logger;
        _apiClient = apiClient;
    }

    public async Task<IActionResult> Index()
    {
        var token = User.Claims.FirstOrDefault(c => c.Type == "ApiToken")?.Value;
        if (string.IsNullOrWhiteSpace(token))
        {
            return RedirectToAction("Login", "Account");
        }

        var items = await _apiClient.GetTasksAsync(token);
        return View(new HomeIndexViewModel { Items = items });
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateTodoViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return RedirectToAction(nameof(Index));
        }

        var token = User.Claims.FirstOrDefault(c => c.Type == "ApiToken")?.Value;
        if (string.IsNullOrWhiteSpace(token))
        {
            return RedirectToAction("Login", "Account");
        }

        await _apiClient.CreateTaskAsync(token, model);
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    public async Task<IActionResult> Delete(int id)
    {
        var token = User.Claims.FirstOrDefault(c => c.Type == "ApiToken")?.Value;
        if (string.IsNullOrWhiteSpace(token))
        {
            return RedirectToAction("Login", "Account");
        }

        await _apiClient.DeleteTaskAsync(token, id);
        return RedirectToAction(nameof(Index));
    }

    [AllowAnonymous]
    public IActionResult Privacy()
    {
        return View();
    }

    [AllowAnonymous]
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
