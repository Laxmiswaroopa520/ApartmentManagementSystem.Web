using Microsoft.AspNetCore.Mvc;

namespace ApartmentManagementSystem.Web.Controllers
{
    public class AuthController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
