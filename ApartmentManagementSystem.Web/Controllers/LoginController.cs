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

    [HttpGet]
    [AllowAnonymous]
    public IActionResult Index()
    {
        // Redirect if already logged in
        if (User.Identity?.IsAuthenticated == true)
            return RedirectToAction("Index", "Dashboard");

        return View();
    }

    [HttpPost]
    [AllowAnonymous]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Index(LoginViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        try
        {
            var request = new LoginRequest
            {
                Username = model.Username,
                Password = model.Password
            };

            
            var response = await AuthApiService.LoginAsync(request);

            if (response?.Success == true && response.Data != null)
            {
                // Store JWT token for API calls
                Response.Cookies.Append("AuthToken", response.Data.Token, new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.Strict,
                    Expires = DateTimeOffset.UtcNow.AddHours(24)
                });

                // Store user info for display
                Response.Cookies.Append("UserName", response.Data.FullName, new CookieOptions
                {
                    Expires = DateTimeOffset.UtcNow.AddHours(24)
                });

                Response.Cookies.Append("UserRole", response.Data.Role, new CookieOptions
                {
                    Expires = DateTimeOffset.UtcNow.AddHours(24)
                });

                Response.Cookies.Append("UserId", response.Data.UserId.ToString(), new CookieOptions
                {
                    Expires = DateTimeOffset.UtcNow.AddHours(24)
                });

                // CREATE MVC AUTHENTICATION COOKIE (THIS WAS MISSING!)
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, response.Data.UserId.ToString()),
                    new Claim(ClaimTypes.Name, response.Data.FullName),
                    new Claim(ClaimTypes.Role, response.Data.Role),
                    new Claim("Username", model.Username)
                };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

                var authProperties = new AuthenticationProperties
                {
                    IsPersistent = model.RememberMe,
                    ExpiresUtc = DateTimeOffset.UtcNow.AddHours(24),
                    AllowRefresh = true
                };

                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    claimsPrincipal,
                    authProperties);

                // Success message and redirect
                TempData["SuccessMessage"] = $"Welcome back, {response.Data.FullName}!";
                return RedirectToAction("Index", "Dashboard");
            }

            ModelState.AddModelError(string.Empty, response?.Message ?? "Login failed");
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, $"Error: {ex.Message}");
        }

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Logout()
    {
        // Sign out from MVC authentication
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

        // Delete all cookies
        Response.Cookies.Delete("AuthToken");
        Response.Cookies.Delete("UserName");
        Response.Cookies.Delete("UserRole");
        Response.Cookies.Delete("UserId");

        TempData["SuccessMessage"] = "Logged out successfully";
        return RedirectToAction("Index");
    }
}