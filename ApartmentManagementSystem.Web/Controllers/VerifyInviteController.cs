using ApartmentManagementSystem.Web.Constants;
using ApartmentManagementSystem.Web.Services;
using ApartmentManagementSystem.Web.Services.DTOs.Onboarding;
using ApartmentManagementSystem.Web.ViewModels.Onboarding;
using Microsoft.AspNetCore.Mvc;

namespace ApartmentManagementSystem.Web.Controllers;

/// <summary>
/// Controller responsible for verifying onboarding invite using OTP.
/// 
/// Responsibilities:
/// - Display OTP verification page
/// - Validate OTP entered by user
/// - Redirect verified users to Complete Registration flow
/// </summary>
public class VerifyInviteController : Controller
{
    /// <summary>
    /// Service used to communicate with Onboarding API for OTP verification.
    /// </summary>
    private readonly OnboardingApiService OnboardingApiService;

    /// <summary>
    /// Constructor for injecting OnboardingApiService dependency.
    /// </summary>
    /// <param name="onboardingApiService">
    /// Service responsible for onboarding verification operations.
    /// </param>
    public VerifyInviteController(OnboardingApiService onboardingApiService)
    {
        OnboardingApiService = onboardingApiService;
    }

    /// <summary>
    /// Displays the OTP verification page.
    /// </summary>
    /// <returns>
    /// OTP verification view.
    /// </returns>
    [HttpGet]
    public IActionResult Index()
    {
        return View();
    }

    /// <summary>
    /// Handles OTP verification request.
    /// </summary>
    /// <param name="model">
    /// VerifyOtpViewModel containing phone number and OTP code.
    /// </param>
    /// <returns>
    /// Redirects to CompleteRegistration page if verification succeeds,
    /// otherwise returns the same view with validation errors.
    /// </returns>
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
            // Store verified data temporarily for next step
            TempData["VerifiedPhone"] = model.PrimaryPhone;
            TempData["FullName"] = response.Data.FullName;

            return RedirectToAction("Index", "CompleteRegistration");
        }

        ModelState.AddModelError("", response?.Message ?? ErrorMessages.OtpVerificationFailed);

        return View(model);
    }
}






































/*using ApartmentManagementSystem.Web.Services;
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


*/



















