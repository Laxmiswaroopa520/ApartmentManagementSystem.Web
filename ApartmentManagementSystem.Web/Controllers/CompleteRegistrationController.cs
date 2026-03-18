using ApartmentManagementSystem.Web.Constants;
using ApartmentManagementSystem.Web.Services;
using ApartmentManagementSystem.Web.Services.DTOs.Onboarding;
using ApartmentManagementSystem.Web.ViewModels.Onboarding;
using Microsoft.AspNetCore.Mvc;

namespace ApartmentManagementSystem.Web.Controllers
{
    /// <summary>
    /// Handles the final step of resident self-registration.
    ///
    /// Requires a verified phone number passed via TempData from VerifyInviteController.
    /// Creates the resident's login credentials and sets their status to PendingFlatAllocation.
    /// No [Authorize] attribute — the user does not have an account yet at this stage.
    /// </summary>
    public class CompleteRegistrationController : Controller
    {
        private readonly OnboardingApiService OnboardingApiService;

        /// <summary>
        /// Initialises the controller with the onboarding API service.
        /// </summary>
        public CompleteRegistrationController(OnboardingApiService onboardingApiService)
        {
            OnboardingApiService = onboardingApiService;
        }

        /// <summary>
        /// Displays the registration completion form.
        ///
        /// Reads the verified phone number and full name from TempData
        /// (set by VerifyInviteController after successful OTP verification).
        /// Keeps the TempData values alive for the POST.
        /// Redirects to VerifyInvite/Index if TempData is missing.
        /// </summary>
        [HttpGet]
        public IActionResult Index()
        {
            var verifiedPhone = TempData[AppMessages.TempVerifiedPhone]?.ToString();
            var fullName = TempData[AppMessages.TempFullName]?.ToString();

            if (string.IsNullOrEmpty(verifiedPhone))
            {
                TempData[AppMessages.ErrorMessage] = AppMessages.OtpVerifyFirst;
                return RedirectToAction("Index", "VerifyInvite");
            }

            // Keep values alive for the POST so they survive the redirect
            TempData.Keep(AppMessages.TempVerifiedPhone);
            TempData.Keep(AppMessages.TempFullName);

            return View(new CompleteRegistrationViewModel
            {
                PrimaryPhone = verifiedPhone,
                FullName = fullName ?? string.Empty
            });
        }

        /// <summary>
        /// Processes the registration completion form.
        ///
        /// Sends credentials and profile data to the API.
        /// On success: stores the success message in TempData and redirects to Login.
        /// On failure: re-renders the form with a model-level error message.
        /// </summary>
        /// <param name="model">Registration form data including username, password, and contact details.</param>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(CompleteRegistrationViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            try
            {
                var response = await OnboardingApiService.CompleteRegistrationAsync(
                    new CompleteRegistrationRequest
                    {
                        PrimaryPhone = model.PrimaryPhone,
                        FullName = model.FullName,
                        SecondaryPhone = model.SecondaryPhone,
                        Email = model.Email,
                        Username = model.Username,
                        Password = model.Password
                    });

                if (response?.Success == true && response.Data != null)
                {
                    TempData[AppMessages.SuccessMessage] = response.Data.Message;
                    return RedirectToAction("Index", "Login");
                }

                ModelState.AddModelError(string.Empty,
                    response?.Message ?? AppMessages.RegistrationFailed);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
            }

            return View(model);
        }
    }
}











































/*using ApartmentManagementSystem.Web.Constants;
using ApartmentManagementSystem.Web.Services;
using ApartmentManagementSystem.Web.Services.DTOs.Onboarding;
using ApartmentManagementSystem.Web.ViewModels.Onboarding;
using Microsoft.AspNetCore.Mvc;

namespace ApartmentManagementSystem.Web.Controllers
{
    /// <summary>
    /// Handles the final step of resident self-registration.
    /// Requires a verified phone number passed via TempData from VerifyInviteController.
    /// </summary>
    public class CompleteRegistrationController : Controller
    {
        private readonly OnboardingApiService OnboardingApiService;

        public CompleteRegistrationController(OnboardingApiService onboardingApiService)
        {
            OnboardingApiService = onboardingApiService;
        }

        /// <summary>
        /// Displays the registration completion form.
        /// Redirects back to OTP step if VerifiedPhone is missing from TempData.
        /// </summary>
        [HttpGet]
        public IActionResult Index()
        {
            var verifiedPhone = TempData[AppMessages.TempVerifiedPhone]?.ToString();
            var fullName = TempData[AppMessages.TempFullName]?.ToString();

            if (string.IsNullOrEmpty(verifiedPhone))
            {
                TempData[AppMessages.ErrorMessage] = AppMessages.OtpVerifyFirst;
                return RedirectToAction("Index", "VerifyInvite");
            }

            // Keep values alive for the POST
            TempData.Keep(AppMessages.TempVerifiedPhone);
            TempData.Keep(AppMessages.TempFullName);

            return View(new CompleteRegistrationViewModel
            {
                PrimaryPhone = verifiedPhone,
                FullName = fullName ?? string.Empty
            });
        }

        /// <summary>
        /// Processes the registration form and creates the resident account.
        /// On success: redirects to Login.
        /// On failure: re-renders form with validation errors.
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(CompleteRegistrationViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            try
            {
                var response = await OnboardingApiService.CompleteRegistrationAsync(
                    new CompleteRegistrationRequest
                    {
                        PrimaryPhone = model.PrimaryPhone,
                        FullName = model.FullName,
                        SecondaryPhone = model.SecondaryPhone,
                        Email = model.Email,
                        Username = model.Username,
                        Password = model.Password
                    });

                if (response?.Success == true && response.Data != null)
                {
                    TempData[AppMessages.SuccessMessage] = response.Data.Message;
                    return RedirectToAction("Index", "Login");
                }

                ModelState.AddModelError(string.Empty, response?.Message ?? AppMessages.RegistrationFailed);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
            }

            return View(model);
        }
    }
}
*/




















