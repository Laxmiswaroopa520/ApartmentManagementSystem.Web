using ApartmentManagementSystem.Web.Services;
using ApartmentManagementSystem.Web.ViewModels.Auth;
using Microsoft.AspNetCore.Mvc;

namespace ApartmentManagementSystem.Web.Controllers
{
    public class RegistrationController : Controller
    {
        private readonly OnboardingApiService OnBoardingService;

        public RegistrationController(OnboardingApiService onboarding)
        {
            OnBoardingService = onboarding;
        }

        [HttpGet]
        public IActionResult Index(string email)
        {
            return View(new CompleteRegistrationViewModel { Email = email });
        }

        [HttpPost]
        public async Task<IActionResult> Index(CompleteRegistrationViewModel vm)
        {
            await OnBoardingService.CompleteRegistrationAsync(vm);
            return RedirectToAction("Index", "Login");
        }
    }
}
