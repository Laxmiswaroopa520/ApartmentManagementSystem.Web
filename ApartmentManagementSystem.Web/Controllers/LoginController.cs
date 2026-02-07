using ApartmentManagementSystem.Web.Services;
using ApartmentManagementSystem.Web.Services.DTOs.Login;
using ApartmentManagementSystem.Web.ViewModels.Auth;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ApartmentManagementSystem.Web.Controllers;

[AllowAnonymous]
public class LoginController : Controller
{
    private readonly AuthApiService AuthApiService;

    public LoginController(AuthApiService authApiService)
    {
        AuthApiService = authApiService;
    }

    // GET: Login
    [HttpGet]
    public IActionResult Index()
    {
        if (User.Identity?.IsAuthenticated == true)
            return RedirectToAction("Index", "Dashboard");

        return View();
    }

    // POST: Login
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Index(LoginViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        var request = new LoginRequest
        {
            Username = model.Username,
            Password = model.Password
        };

        var response = await AuthApiService.LoginAsync(request);

        // THIS IS THE MISSING LOGIC
        if (response == null || !response.Success)
        {
            // Redirect to inactive page
            if (response?.ErrorCode == "ACCOUNT_INACTIVE")
            {
                return RedirectToAction(nameof(Inactive));
            }

            // Other login errors stay on login page
            ModelState.AddModelError(string.Empty,
                response?.Message ?? "Login failed");

            return View(model);
        }
        // SUCCESSFUL LOGIN
        // Store JWT token
        Response.Cookies.Append("AuthToken", response.Data.Token, new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Strict,
            Expires = DateTimeOffset.UtcNow.AddHours(24)
        });

        Response.Cookies.Append("UserName", response.Data.FullName,
            new CookieOptions { Expires = DateTimeOffset.UtcNow.AddHours(24) });

        Response.Cookies.Append("UserRole", response.Data.Role,
            new CookieOptions { Expires = DateTimeOffset.UtcNow.AddHours(24) });

        Response.Cookies.Append("UserId", response.Data.UserId.ToString(),
            new CookieOptions { Expires = DateTimeOffset.UtcNow.AddHours(24) });

        // MVC authentication cookie
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, response.Data.UserId.ToString()),
            new Claim(ClaimTypes.Name, response.Data.FullName),
            new Claim(ClaimTypes.Role, response.Data.Role),
            new Claim("Username", model.Username)
        };

        var identity = new ClaimsIdentity(
            claims,
            CookieAuthenticationDefaults.AuthenticationScheme);

        await HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            new ClaimsPrincipal(identity),
            new AuthenticationProperties
            {
                IsPersistent = model.RememberMe,
                ExpiresUtc = DateTimeOffset.UtcNow.AddHours(24)
            });

        TempData["SuccessMessage"] = $"Welcome back, {response.Data.FullName}!";
        return RedirectToAction("Index", "Dashboard");
    }

    // INACTIVE ACCOUNT PAGE
    [HttpGet]
    public IActionResult Inactive()
    {
        return View();
    }

    // LOGOUT
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(
            CookieAuthenticationDefaults.AuthenticationScheme);

        Response.Cookies.Delete("AuthToken");
        Response.Cookies.Delete("UserName");
        Response.Cookies.Delete("UserRole");
        Response.Cookies.Delete("UserId");

        TempData["SuccessMessage"] = "Logged out successfully";
        return RedirectToAction(nameof(Index));
    }
}























