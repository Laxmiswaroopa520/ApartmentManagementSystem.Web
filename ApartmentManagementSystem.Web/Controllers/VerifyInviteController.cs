using ApartmentManagementSystem.Web.Services;
using ApartmentManagementSystem.Web.ViewModels.Auth;
using Microsoft.AspNetCore.Mvc;

namespace ApartmentManagementSystem.Web.Controllers
{
    public class VerifyInviteController : Controller
    {
        private readonly OnboardingApiService OnboardingService;

        public VerifyInviteController(OnboardingApiService onboarding)
        {
            OnboardingService = onboarding;
        }

        [HttpGet]
        public IActionResult Index() => View();

        [HttpPost]
        public async Task<IActionResult> Index(VerifyInviteViewModel vm)
        {
            await OnboardingService.VerifyOtpAsync(vm);
            return RedirectToAction("Index", "Registration", new { email = vm.Email });
        }
    }
}