using ApartmentManagementSystem.Web.Services;
//using ApartmentManagementSystem.Web.Services;
using ApartmentManagementSystem.Web.Services.DTOs.Onboarding;
using ApartmentManagementSystem.Web.ViewModels.Auth;
using ApartmentManagementSystem.Web.ViewModels.Onboarding;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Mvc;

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






















/*
[AllowAnonymous] // Public page - no auth required
public class VerifyInviteController : Controller
{
    private readonly OnboardingApiService _onboardingApiService;

    public VerifyInviteController(OnboardingApiService onboardingApiService)
    {
        _onboardingApiService = onboardingApiService;
    }

    [HttpGet]
    public IActionResult Index()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Index(VerifyOtpViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        try
        {
            var request = new VerifyOtpRequest
            {
                PrimaryPhone = model.PrimaryPhone,
                OtpCode = model.OtpCode
            };

            var response = await _onboardingApiService.VerifyOtpAsync(request);

            if (response?.Success == true)
            {
                TempData["SuccessMessage"] = response.Message;
                TempData["VerifiedPhone"] = model.PrimaryPhone; // Pass to next page
                return RedirectToAction("Index", "CompleteRegistration");
            }

            ModelState.AddModelError(string.Empty, response?.Message ?? "OTP verification failed");
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, $"Error: {ex.Message}");
        }

        return View(model);
    }
}



*/






