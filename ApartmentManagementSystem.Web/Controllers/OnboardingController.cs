using ApartmentManagementSystem.Web.Mappers.Onboarding;
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

    // GET
    [HttpGet]
    public async Task<IActionResult> Create()
    {
        var model = new CreateInviteViewModel();
        await LoadResidentTypesAsync(model);
        return View(model);
    }

    // POST
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateInviteViewModel model)
    {
        if (!ModelState.IsValid)
        {
            await LoadResidentTypesAsync(model);
            return View(model);
        }

        var request = new CreateInviteRequest
        {
            FullName = model.FullName,
            PrimaryPhone = model.PrimaryPhone,
            ResidentType = model.ResidentType
        };

        var response = await OnboardingApiService.CreateInviteAsync(request);

        if (response?.Success == true && response.Data != null)
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
        await LoadResidentTypesAsync(model);
        return View(model);
    }

    // Helper method (single source)
    private async Task LoadResidentTypesAsync(CreateInviteViewModel model)
    {
        var response = await OnboardingApiService.GetResidentTypesAsync();

        model.ResidentTypes = response?.Success == true && response.Data != null
            ? ResidentTypeViewModelMapper.From(response.Data)
            : new List<ResidentTypeOption>();
    }
}
    



















