using ApartmentManagementSystem.Web.Services;
//using ApartmentManagementSystem.Web.Services;
using ApartmentManagementSystem.Web.Services.DTOs.Onboarding;
using ApartmentManagementSystem.Web.ViewModels.Auth;
using ApartmentManagementSystem.Web.ViewModels.Onboarding;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Mvc;

namespace ApartmentManagementSystem.Web.Controllers;

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











/*
namespace ApartmentManagementSystem.Web.Controllers
{
    //   ApartmentManagementSystem.Web.Services;
    using ApartmentManagementSystem.Web.ViewModels.Onboarding;
    using global::ApartmentManagementSystem.Web.Services.DTOs.Onboarding;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    //namespace ApartmentManagementSystem.Web.Controllers;

    public class VerifyInviteController : Controller
    {
        private readonly OnboardingApiService _onboardingApiService;

        public VerifyInviteController(OnboardingApiService onboardingApiService)
        {
            _onboardingApiService = onboardingApiService;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
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
                    TempData["VerifiedPhone"] = model.PrimaryPhone;
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
}
*/