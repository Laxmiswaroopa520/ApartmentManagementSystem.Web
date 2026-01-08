using ApartmentManagementSystem.Web.DTOs.Auth;
using ApartmentManagementSystem.Web.Services;
using ApartmentManagementSystem.Web.ViewModels.Auth;
using Microsoft.AspNetCore.Mvc;

namespace ApartmentManagementSystem.Web.Controllers;

public class LoginController : Controller
{
    private readonly AuthApiService AuthService;
    private readonly AuthApiClient _authApiClient;

    public LoginController(AuthApiService auth, AuthApiClient authApiClient)
    {
        AuthService = auth;
        _authApiClient = authApiClient;
    }

    [HttpGet]
    public IActionResult Index() => View();

    [HttpPost]
    public async Task<IActionResult> Login(LoginViewDto model)
    {
        var result = await _authApiClient.LoginAsync(model);

        if (result == null)
        {
            ModelState.AddModelError("", "Invalid login");
            return View(model);
        }

        HttpContext.Session.SetString("JWT", result.Token);
        return RedirectToAction("Index", "Dashboard");
    }


    /*  public async Task<IActionResult> Index(LoginViewModel vm)
      {
          var token = await _auth.LoginAsync(vm);

          // TEMP: store token in session/cookie
          HttpContext.Session.SetString("JWT", token);

          return RedirectToAction("Index", "Dashboard");
      }*/
}
