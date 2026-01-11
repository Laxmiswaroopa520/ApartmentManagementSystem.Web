using ApartmentManagementSystem.Web.Services;
using ApartmentManagementSystem.Web.Services.DTOs.Onboarding;
using ApartmentManagementSystem.Web.ViewModels.Onboarding;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Mvc;

namespace ApartmentManagementSystem.Web.Controllers;

[AllowAnonymous] // Public page - no auth required
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
        var phone = TempData["VerifiedPhone"]?.ToString();
        if (string.IsNullOrEmpty(phone))
        {
            TempData["ErrorMessage"] = "Please verify OTP first";
            return RedirectToAction("Index", "VerifyInvite");
        }

        // Keep phone in TempData for POST
        TempData.Keep("VerifiedPhone");

        var model = new CompleteRegistrationViewModel
        {
            PrimaryPhone = phone
        };

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Index(CompleteRegistrationViewModel model)
    {
        if (!ModelState.IsValid)
        {
            TempData.Keep("VerifiedPhone"); // Keep for re-render
            return View(model);
        }

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

            if (response?.Success == true)
            {
                TempData["SuccessMessage"] = "Registration completed! Please login with your credentials.";
                TempData["NewUsername"] = model.Username; // Pre-fill login form
                return RedirectToAction("Index", "Login");
            }

            ModelState.AddModelError(string.Empty, response?.Message ?? "Registration failed");
            TempData.Keep("VerifiedPhone"); // Keep for re-render
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, $"Error: {ex.Message}");
            TempData.Keep("VerifiedPhone"); // Keep for re-render
        }

        return View(model);
    }
}










/*
namespace ApartmentManagementSystem.Web.Controllers
{
    using ApartmentManagementSystem.Web.Services;
    using ApartmentManagementSystem.Web.ViewModels.Onboarding;
    // using global::ApartmentManagementSystem.Web.Services;
    using global::ApartmentManagementSystem.Web.Services.DTOs.Onboarding;
    using global::ApartmentManagementSystem.Web.ViewModels.Auth;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    // namespace ApartmentManagementSystem.Web.Controllers;

    public class CompleteRegistrationController : Controller
    {
        private readonly OnboardingApiService _onboardingApiService;

        public CompleteRegistrationController(OnboardingApiService onboardingApiService)
        {
            _onboardingApiService = onboardingApiService;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Index()
        {
            var phone = TempData["VerifiedPhone"]?.ToString();
            if (string.IsNullOrEmpty(phone))
            {
                TempData["ErrorMessage"] = "Please verify OTP first";
                return RedirectToAction("Index", "VerifyInvite");
            }

            var model = new CompleteRegistrationViewModel
            {
                PrimaryPhone = phone
            };

            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
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

                if (response?.Success == true)
                {
                    TempData["SuccessMessage"] = "Registration completed! Please login with your credentials.";
                    return RedirectToAction("Index", "Login");
                }

                ModelState.AddModelError(string.Empty, response?.Message ?? "Registration failed");
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