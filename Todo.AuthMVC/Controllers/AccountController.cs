using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Todo.AuthMVC.Services;
using Todo.AuthMVC.ViewModels;

namespace Todo.AuthMVC.Controllers;

[AllowAnonymous]
public class AccountController : Controller
{
    private readonly ApiClient _apiClient;

    public AccountController(ApiClient apiClient)
    {
        _apiClient = apiClient;
    }

    [HttpGet]
    public IActionResult Login() => View(new LoginViewModel());

    [HttpPost]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (!ModelState.IsValid) return View(model);

        var result = await _apiClient.LoginAsync(model);
        if (!result.Success || string.IsNullOrWhiteSpace(result.Token))
        {
            ModelState.AddModelError(string.Empty, result.Message ?? "Login xato.");
            return View(model);
        }

        await SignInAsync(model.Username, result.Token);
        return RedirectToAction("Index", "Home");
    }

    [HttpGet]
    public IActionResult Register() => View(new RegisterViewModel());

    [HttpPost]
    public async Task<IActionResult> Register(RegisterViewModel model)
    {
        if (!ModelState.IsValid) return View(model);

        var result = await _apiClient.RegisterAsync(model);
        if (!result.Success || string.IsNullOrWhiteSpace(result.Token))
        {
            ModelState.AddModelError(string.Empty, result.Message ?? "Ro'yxatdan o'tish xato.");
            return View(model);
        }

        await SignInAsync(model.Username, result.Token);
        return RedirectToAction("Index", "Home");
    }

    [HttpPost]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction(nameof(Login));
    }

    private async Task SignInAsync(string username, string token)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.Name, username),
            new("ApiToken", token)
        };

        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        var principal = new ClaimsPrincipal(identity);

        await HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            principal,
            new AuthenticationProperties { IsPersistent = true, ExpiresUtc = DateTimeOffset.UtcNow.AddHours(8) });
    }
}
