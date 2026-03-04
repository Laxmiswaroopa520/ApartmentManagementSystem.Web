using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using ApartmentManagementSystem.Web.Models;
using Microsoft.AspNetCore.Authorization;
namespace ApartmentManagementSystem.Web.Controllers;

[AllowAnonymous]
public class HomeController : Controller
{
    [AllowAnonymous]
    public IActionResult Index()
    {
        // If user is authenticated, go to dashboard
        if (User.Identity?.IsAuthenticated == true)
            return RedirectToAction("Index", "Dashboard");
        return View();
       
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel
        {
            RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
        });
    }
}

























