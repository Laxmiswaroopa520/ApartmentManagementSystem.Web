using ApartmentManagementSystem.Web.Services;
using ApartmentManagementSystem.Web.Services.DTOs.Onboarding;
using ApartmentManagementSystem.Web.ViewModels.Onboarding;
using Microsoft.AspNetCore.Mvc;
namespace ApartmentManagementSystem.Web.Controllers;


using ApartmentManagementSystem.Web.Services;
using ApartmentManagementSystem.Web.Services.DTOs.Onboarding;
using ApartmentManagementSystem.Web.ViewModels.Onboarding;
using Microsoft.AspNetCore.Mvc;


public class CompleteRegistrationController : Controller
{
    private readonly OnboardingApiService _onboardingApiService;

    public CompleteRegistrationController(OnboardingApiService onboardingApiService)
    {
        _onboardingApiService = onboardingApiService;
    }

    [HttpGet]
    public IActionResult Index()
    {
        // Get verified phone from TempData
        var verifiedPhone = TempData["VerifiedPhone"]?.ToString();
        var fullName = TempData["FullName"]?.ToString();

        if (string.IsNullOrEmpty(verifiedPhone))
        {
            TempData["ErrorMessage"] = "Please verify OTP first";
            return RedirectToAction("Index", "VerifyInvite");
        }

        // Preserve for the POST
        TempData.Keep("VerifiedPhone");
        TempData.Keep("FullName");

        var model = new CompleteRegistrationViewModel
        {
            PrimaryPhone = verifiedPhone,
            FullName = fullName ?? string.Empty
        };

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Index(CompleteRegistrationViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        try
        {
            var request = new CompleteRegistrationRequest
            {
                PrimaryPhone = model.PrimaryPhone,
                FullName = model.FullName,
                SecondaryPhone = model.SecondaryPhone,
                Email = model.Email,
                Username = model.Username,
                Password = model.Password
            };

            var response = await _onboardingApiService.CompleteRegistrationAsync(request);

            if (response?.Success == true && response.Data != null)
            {
                TempData["SuccessMessage"] = response.Data.Message;
                return RedirectToAction("Index", "Login"); // Or wherever you want to redirect
            }

            ModelState.AddModelError("", response?.Message ?? "Registration failed");
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", $"Error: {ex.Message}");
        }

        return View(model);
    }
}













