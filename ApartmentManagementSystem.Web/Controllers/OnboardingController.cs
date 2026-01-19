/*using ApartmentManagementSystem.Web.Services;
using ApartmentManagementSystem.Web.Services.DTOs.Onboarding;
using ApartmentManagementSystem.Web.ViewModels.Onboarding;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

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
        //Add a new CompleteRegistration GET action that populates Floors
        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> CompleteRegistration(string phone)
        {
            if (string.IsNullOrEmpty(phone))
                return RedirectToAction("Create"); // fallback

            // Call API to get floors
            var floorsApiResponse = await _onboardingApiService.GetFloorsAsync();
            var floorsList = floorsApiResponse.Select(f => new SelectListItem
            {
                Value = f.Id.ToString(),
                Text = f.Name
            }).ToList();

            var model = new CompleteRegistrationViewModel
            {
                PrimaryPhone = phone,
                Floors = floorsList
            };

            return View(model);
        }
        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CompleteRegistration(CompleteRegistrationViewModel model)
        {
            if (!ModelState.IsValid)
            {
                // reload floors for dropdown
                var floorsApiResponse = await _onboardingApiService.GetFloorsAsync();
                model.Floors = floorsApiResponse.Select(f => new SelectListItem
                {
                    Value = f.Id.ToString(),
                    Text = f.Name
                }).ToList();

                return View(model);
            }

            // Map VM to DTO
            var dto = new CompleteRegistrationDto
            {
                PrimaryPhone = model.PrimaryPhone,
                FullName = model.FullName,
                Email = model.Email,
                SecondaryPhone = model.SecondaryPhone,
                Username = model.Username,
                Password = model.Password,
                FloorId = model.FloorId,
                FlatId = model.FlatId
            };

            var result = await _onboardingApiService.CompleteRegistrationAsync(dto);

            if (result.Success)
                return RedirectToAction("RegistrationSuccess");

            ModelState.AddModelError(string.Empty, result.Message);
            return View(model);
        }


    }
}
*/



using ApartmentManagementSystem.Web.Services;
using ApartmentManagementSystem.Web.Services.DTOs.Onboarding;
using ApartmentManagementSystem.Web.ViewModels.Onboarding;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

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

        // ---------------- CREATE INVITE ----------------
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var model = new CreateInviteViewModel
            {
                AvailableRoles = await _onboardingApiService.GetAvailableRolesAsync()
            };

            return View(model);
        }

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

        // ---------------- COMPLETE REGISTRATION ----------------

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> CompleteRegistration(string phone)
        {
            if (string.IsNullOrEmpty(phone))
                return RedirectToAction(nameof(Create));

            var floors = await _onboardingApiService.GetFloorsAsync();

            var model = new CompleteRegistrationViewModel
            {
                PrimaryPhone = phone,
                Floors = floors.Select(f => new SelectListItem
                {
                    Value = f.Id.ToString(),
                    Text = f.Name
                }).ToList()
            };

            return View(model);
        }

        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CompleteRegistration(CompleteRegistrationViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var floors = await _onboardingApiService.GetFloorsAsync();
                model.Floors = floors.Select(f => new SelectListItem
                {
                    Value = f.Id.ToString(),
                    Text = f.Name
                }).ToList();

                return View(model);
            }

            // ✅ USE WEB DTO (NOT API DTO)
            var request = new CompleteRegistrationRequest
            {
                PrimaryPhone = model.PrimaryPhone,
                FullName = model.FullName,
                Email = model.Email,
                SecondaryPhone = model.SecondaryPhone,
                Username = model.Username,
                Password = model.Password,
                FloorId = model.FloorId,
                FlatId = model.FlatId
            };

            var result = await _onboardingApiService.CompleteRegistrationAsync(request);

            if (result?.Success == true)
                return RedirectToAction("RegistrationSuccess");

            ModelState.AddModelError(string.Empty, result?.Message ?? "Registration failed");
            return View(model);
        }
    }
}

