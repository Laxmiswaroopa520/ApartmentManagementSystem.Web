using ApartmentManagementSystem.Web.Services;
using ApartmentManagementSystem.Web.Services.DTOs.Onboarding;
using ApartmentManagementSystem.Web.ViewModels.Onboarding;
using Microsoft.AspNetCore.Mvc;
namespace ApartmentManagementSystem.Web.Controllers;
public class VerifyInviteController : Controller
{
    private readonly OnboardingApiService OnboardingApiService;

    public VerifyInviteController(OnboardingApiService onboardingApiService)
    {
        OnboardingApiService = onboardingApiService;
    }

    [HttpGet]
    public IActionResult Index()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Index(VerifyOtpViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        var request = new VerifyOtpRequest
        {
            PrimaryPhone = model.PrimaryPhone,
            OtpCode = model.OtpCode
        };

        var response = await OnboardingApiService.VerifyOtpAsync(request);

        if (response?.Success == true && response.Data != null)
        {
            TempData["VerifiedPhone"] = model.PrimaryPhone;
            TempData["FullName"] = response.Data.FullName;
            return RedirectToAction("Index", "CompleteRegistration");
        }

        ModelState.AddModelError("", response?.Message ?? "OTP verification failed");
        return View(model);
    }
}






















