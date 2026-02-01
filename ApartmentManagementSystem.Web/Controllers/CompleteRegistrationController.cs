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








/*
public class CompleteRegistrationController : Controller
{
    private readonly OnboardingApiService OnboardingApiService;

    public CompleteRegistrationController(OnboardingApiService onboardingApiService)
    {
        OnboardingApiService = onboardingApiService;
    }

    [HttpGet]
    public IActionResult Index()
    {
        var phone = TempData["VerifiedPhone"] as string;
        var fullName = TempData["FullName"] as string;

        if (string.IsNullOrEmpty(phone))
            return RedirectToAction("Index", "VerifyInvite");

        var model = new CompleteRegistrationViewModel
        {
            PrimaryPhone = phone,
            FullName = fullName ?? ""
        };

        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Index(CompleteRegistrationViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        var request = new CompleteRegistrationRequest
        {
            PrimaryPhone = model.PrimaryPhone,
            FullName = model.FullName,
            SecondaryPhone = model.SecondaryPhone,
            Email = model.Email,
            Username = model.Username,
            Password = model.Password
        };

        var response = await OnboardingApiService.CompleteRegistrationAsync(request);

        if (response?.Success == true)
        {
            TempData["SuccessMessage"] = "Registration completed! Awaiting flat assignment from admin. You'll receive an email once assigned.";
            return RedirectToAction("Index", "Login");
        }

        ModelState.AddModelError("", response?.Message ?? "Registration failed");
        return View(model);
    }
}
*/

















/*
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

    /*   [HttpPost]
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
       }-----
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Index(CompleteRegistrationViewModel model)
    {
        if (!ModelState.IsValid)
        {
            TempData.Keep("VerifiedPhone");
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
                Password = model.Password,

                // ✅ NEW
                FloorId = model.FloorId,
                FlatId = model.FlatId
            };

            var response = await _onboardingApiService.CompleteRegistrationAsync(request);

            if (response?.Success == true)
            {
                TempData["SuccessMessage"] = "Registration completed! Please login.";
                TempData["NewUsername"] = model.Username;
                return RedirectToAction("Index", "Login");
            }

            ModelState.AddModelError(string.Empty, response?.Message ?? "Registration failed");
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
        }

        TempData.Keep("VerifiedPhone");
        return View(model);
    }

}
*/








