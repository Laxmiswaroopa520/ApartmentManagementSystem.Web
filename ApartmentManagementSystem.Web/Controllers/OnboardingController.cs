

/*
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

*/

using ApartmentManagementSystem.Web.Services;
using ApartmentManagementSystem.Web.Services.DTOs.Onboarding;
using ApartmentManagementSystem.Web.ViewModels.Onboarding;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace ApartmentManagementSystem.Web.Controllers;

[Authorize(Roles = "SuperAdmin,Manager")]
public class OnboardingController : Controller
{
    private readonly OnboardingApiService OnboardingApiService;

    public OnboardingController(OnboardingApiService onboardingApiService)
    {
        OnboardingApiService = onboardingApiService;
    }

    [HttpGet]
    public IActionResult Create()
    {
        var model = new CreateInviteViewModel
        {
            ResidentTypes = GetResidentTypes()
        };
        return View(model);
    }
    /*
        [HttpPost]
        public async Task<IActionResult> Create(CreateInviteViewModel model)
        {
            if (model.ResidentType <= 0)
            {
                ModelState.AddModelError(nameof(model.ResidentType),
                    "Please select a resident type");

                model.ResidentTypes = new List<ResidentTypeOption>
            {
                new() { Id = 1, Name = "Resident Owner" },
                new() { Id = 2, Name = "Tenant" },
                new() { Id = 3, Name = "Staff" }
            };

                return View(model);
            }

            if (!ModelState.IsValid)
            {
                 model.ResidentTypes = new List<ResidentTypeOption>
                  {
                      new() { Id = 1, Name = "Resident Owner" },
                      new() { Id = 2, Name = "Tenant" },
                      new() { Id = 3, Name = "Staff" }
                  };
                // re-populate dropdown!
              //  model.ResidentTypes = GetResidentTypes();
                return View(model);
            }

            var request = new CreateInviteRequest
            {
                FullName = model.FullName,
                PrimaryPhone = model.PrimaryPhone,
                ResidentType = model.ResidentType
            };

            var response = await _onboardingApiService.CreateInviteAsync(request);

            if (response?.Success == true && response.Data != null)
            {
                var successModel = new InviteSuccessViewModel
                {
                    FullName = response.Data.FullName,
                    PrimaryPhone = response.Data.PrimaryPhone,
                    ResidentType = response.Data.ResidentType,
                    OtpCode = response.Data.OtpCode,
                    Message = response.Data.Message
                };

                return View("Success", successModel);
            }

            ModelState.AddModelError("", response?.Message ?? "Failed to create invite");
            model.ResidentTypes = new List<ResidentTypeOption>
            {
                new() { Id = 1, Name = "Resident Owner" },
                new() { Id = 2, Name = "Tenant" },
                new() { Id = 3, Name = "Staff" }
            };
            return View(model);
        }*/
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateInviteViewModel model)
    {
        if (model.ResidentType < 1 || model.ResidentType > 3)
        {
            ModelState.AddModelError(nameof(model.ResidentType), "Please select a resident type");
            model.ResidentTypes = GetResidentTypes();
            return View(model);
        }

        if (!ModelState.IsValid)
        {
            Console.WriteLine("ModelState is invalid:");
            foreach (var key in ModelState.Keys)
                foreach (var error in ModelState[key].Errors)
                    Console.WriteLine($"{key}: {error.ErrorMessage}");

            model.ResidentTypes = GetResidentTypes();
            return View(model);
        }

      /*  if (!ModelState.IsValid)
        {
            model.ResidentTypes = GetResidentTypes();
            return View(model);
        }*/

        var request = new CreateInviteRequest
        {
            FullName = model.FullName,
            PrimaryPhone = model.PrimaryPhone,
            ResidentType = model.ResidentType   // guaranteed 1–3
        };

        var response = await OnboardingApiService.CreateInviteAsync(request);
      //  var response = await _onboardingApiService.CreateInviteAsync(request);

        if (response != null && response.Success && response.Data != null)
        {
            return View("Success", new InviteSuccessViewModel
            {
                FullName = response.Data.FullName,
                PrimaryPhone = response.Data.PrimaryPhone,
                ResidentType = response.Data.ResidentType,
                OtpCode = response.Data.OtpCode
            });
        }

        // API failed or validation error
        ModelState.AddModelError("", response?.Message ?? "Failed to create invite");
        model.ResidentTypes = GetResidentTypes();
        return View(model);

        /*  if (response?.Success == true && response.Data != null)
          {
              return View("Success", new InviteSuccessViewModel
              {
                  FullName = response.Data.FullName,
                  PrimaryPhone = response.Data.PrimaryPhone,
                  ResidentType = response.Data.ResidentType,
                  OtpCode = response.Data.OtpCode
              });
          }

          ModelState.AddModelError("", response?.Message ?? "Failed to create invite");
          model.ResidentTypes = GetResidentTypes();
          return View(model);*/
    }

    private List<ResidentTypeOption> GetResidentTypes()
    {
        return new List<ResidentTypeOption>
    {
        new() { Id = 1, Name = "Resident Owner" },
        new() { Id = 2, Name = "Tenant" },
        new() { Id = 3, Name = "Staff" }
    };
    }


}
