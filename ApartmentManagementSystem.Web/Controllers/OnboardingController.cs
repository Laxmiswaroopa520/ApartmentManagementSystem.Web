using Microsoft.AspNetCore.Mvc;

namespace ApartmentManagementSystem.Web.Controllers
{
    using ApartmentManagementSystem.Web.Services;
    using ApartmentManagementSystem.Web.ViewModels.Onboarding;
    using global::ApartmentManagementSystem.Web.Services;
    using global::ApartmentManagementSystem.Web.Services.DTOs.Onboarding;
    using global::ApartmentManagementSystem.Web.ViewModels.Onboarding;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    //namespace ApartmentManagementSystem.Web.Controllers;

    [Authorize(Roles = "SuperAdmin,President,Secretary")] // Only these can onboard
    public class OnboardingController : Controller
    {
        private readonly OnboardingApiService _onboardingApiService;

        public OnboardingController(OnboardingApiService onboardingApiService)
        {
            _onboardingApiService = onboardingApiService;
        }

        // GET: /Onboarding/Create
        [HttpGet]
        public IActionResult Create()
        {
            var model = new CreateInviteViewModel
            {
                AvailableRoles = GetAvailableRoles()
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
                model.AvailableRoles = GetAvailableRoles();
                return View(model);
            }

            try
            {
                var request = new CreateInviteRequest
                {
                    FullName = model.FullName,
                    PrimaryPhone = model.PrimaryPhone,
                    RoleId = model.RoleId
                };

                var response = await _onboardingApiService.CreateInviteAsync(request);

                if (response?.Success == true && response.Data != null)
                {
                    // Store OTP and user info in TempData to show on success page
                    TempData["InviteSuccess"] = true;
                    TempData["InvitedUserName"] = response.Data.FullName;
                    TempData["InvitedPhone"] = response.Data.PrimaryPhone;
                    TempData["GeneratedOTP"] = response.Data.OtpCode;

                    return RedirectToAction("Success");
                }

                ModelState.AddModelError(string.Empty, response?.Message ?? "Failed to create invite");
                model.AvailableRoles = GetAvailableRoles();
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"Error: {ex.Message}");
                model.AvailableRoles = GetAvailableRoles();
            }

            return View(model);
        }

        // GET: /Onboarding/Success
        [HttpGet]
        public IActionResult Success()
        {
            // Check if we came from Create action
            if (TempData["InviteSuccess"] == null)
            {
                return RedirectToAction("Create");
            }

            var model = new InviteSuccessViewModel
            {
                FullName = TempData["InvitedUserName"]?.ToString() ?? "",
                PhoneNumber = TempData["InvitedPhone"]?.ToString() ?? "",
                OtpCode = TempData["GeneratedOTP"]?.ToString() ?? ""
            };

            return View(model);
        }

        // GET: /Onboarding/List (Optional - for Phase 2)
        [HttpGet]
        public IActionResult List()
        {
            // TODO: Phase 2 - Show list of all invited/registered users
            return View();
        }

        // Helper method to get available roles
        private List<RoleOption> GetAvailableRoles()
        {
            return new List<RoleOption>
        {
            new RoleOption { Id = Guid.Parse("10000000-0000-0000-0000-000000000005"), Name = "Resident Owner" },
            new RoleOption { Id = Guid.Parse("10000000-0000-0000-0000-000000000006"), Name = "Tenant" },
            new RoleOption { Id = Guid.Parse("10000000-0000-0000-0000-000000000007"), Name = "Security" },
            new RoleOption { Id = Guid.Parse("10000000-0000-0000-0000-000000000008"), Name = "Maintenance Staff" }
        };
        }
    }
}