using ApartmentManagementSystem.Web.Services;
using ApartmentManagementSystem.Web.Services.DTOs.Onboarding;
using ApartmentManagementSystem.Web.ViewModels.Onboarding;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ApartmentManagementSystem.Web.Controllers
{
    [Authorize(Roles = "SuperAdmin,President,Secretary")]
    public class OnboardingController : Controller
    {
        private readonly OnboardingApiService _onboardingApiService;

        public OnboardingController(OnboardingApiService onboardingApiService)
        {
            _onboardingApiService = onboardingApiService;
        }
        

        // GET: /Onboarding/Create
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var model = new CreateInviteViewModel
            {
                AvailableRoles = await _onboardingApiService.GetAvailableRolesAsync()
            };

            return View(model);
        }

        // POST: /Onboarding/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateInviteViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.AvailableRoles = await _onboardingApiService.GetAvailableRolesAsync();
                return View(model);
            }

            var request = new CreateInviteRequest
            {
                FullName = model.FullName,
                PrimaryPhone = model.PrimaryPhone,
                RoleId = model.RoleId
            };

            var response = await _onboardingApiService.CreateInviteAsync(request);

            if (response?.Success == true && response.Data != null)
            {
                var successModel = new InviteSuccessViewModel
                {
                    FullName = response.Data.FullName,
                    PhoneNumber = response.Data.PrimaryPhone,
                    OtpCode = response.Data.OtpCode,
                    GeneratedAt = DateTime.Now
                };

                TempData["InviteSuccessModel"] =
                    System.Text.Json.JsonSerializer.Serialize(successModel);

                return RedirectToAction(nameof(Success));
            }

            ModelState.AddModelError(string.Empty, response?.Message ?? "Failed to create invite");
            model.AvailableRoles = await _onboardingApiService.GetAvailableRolesAsync();

            return View(model);
        }

        /*
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateInviteViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.AvailableRoles = await _onboardingApiService.GetAvailableRolesAsync();
                return View(model);
            }

            var request = new CreateInviteRequest
            {
                FullName = model.FullName,
                PrimaryPhone = model.PrimaryPhone,
                RoleId = model.RoleId
            };

            var response = await _onboardingApiService.CreateInviteAsync(request);

            if (response?.Success == true && response.Data != null)
            {
                TempData["InviteSuccess"] = true;
                TempData["InvitedUserName"] = response.Data.FullName;
                TempData["InvitedPhone"] = response.Data.PrimaryPhone;
                TempData["GeneratedOTP"] = response.Data.OtpCode;

                return RedirectToAction(nameof(Success));
            }

            ModelState.AddModelError(string.Empty, response?.Message ?? "Failed to create invite");
            model.AvailableRoles = await _onboardingApiService.GetAvailableRolesAsync();

            return View(model);
        }
        */
        // GET: /Onboarding/Success
        /*[HttpGet]
        public IActionResult Success()
        {
            if (TempData["InviteSuccess"] == null)
                return RedirectToAction(nameof(Create));

            var model = new InviteSuccessViewModel
            {
                FullName = TempData["InvitedUserName"]?.ToString() ?? "",
                PhoneNumber = TempData["InvitedPhone"]?.ToString() ?? "",
                OtpCode = TempData["GeneratedOTP"]?.ToString() ?? ""
            };

            return View(model);
        */
        [HttpGet]
        public IActionResult Success()
        {
            var json = TempData["InviteSuccessModel"]?.ToString();

            if (string.IsNullOrEmpty(json))
                return RedirectToAction(nameof(Create));

            var model = System.Text.Json.JsonSerializer
                .Deserialize<InviteSuccessViewModel>(json)!;

            return View(model);
        }

      /*  [HttpGet]
        public IActionResult Success()
        {
            // Check without consuming
            if (TempData.Peek("InviteSuccess") == null)             //Reads the value;;  doesn't delete it;;value remains available..
                return RedirectToAction(nameof(Create));
            // now Safely read Values..
            var model = new InviteSuccessViewModel
            {
                FullName = TempData["InvitedUserName"]?.ToString() ?? "",
                PhoneNumber = TempData["InvitedPhone"]?.ToString() ?? "",
                OtpCode = TempData["GeneratedOTP"]?.ToString() ?? "",
                GeneratedAt = DateTime.Now
            };

            return View(model);
        }
      */
    }
}
