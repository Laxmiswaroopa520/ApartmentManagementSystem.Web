using Microsoft.AspNetCore.Mvc;

using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Mvc;

namespace ApartmentManagementSystem.Web.Controllers;

[Authorize] // THIS IS CRITICAL - Protects the entire controller
public class DashboardController : Controller
{
    public IActionResult Index()
    {
        // These come from authentication cookie claims
        var userName = User.Identity?.Name ?? "User";
        var userRole = User.FindFirst(System.Security.Claims.ClaimTypes.Role)?.Value ?? "Unknown";
        var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

        ViewBag.UserName = userName;
        ViewBag.UserRole = userRole;
        ViewBag.UserId = userId;

        return View();
    }
}



/*
namespace ApartmentManagementSystem.Web.Controllers
{
using   Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


    [Authorize]
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            var userName = Request.Cookies["UserName"] ?? "User";
            var userRole = Request.Cookies["UserRole"] ?? "Unknown";

            ViewBag.UserName = userName;
            ViewBag.UserRole = userRole;

            return View();
        }
    }
}
*/