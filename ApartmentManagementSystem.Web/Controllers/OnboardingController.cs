using ApartmentManagementSystem.Web.Mappers.Onboarding;
using ApartmentManagementSystem.Web.Services;
using ApartmentManagementSystem.Web.Services.DTOs.Onboarding;
using ApartmentManagementSystem.Web.ViewModels.Onboarding;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ApartmentManagementSystem.Web.Controllers;

/// <summary>
/// Controller responsible for handling resident onboarding operations.
/// 
/// Accessible only by:
/// SuperAdmin and Manager roles.
/// 
/// Responsibilities:
/// - Creating onboarding invites for new residents
/// - Loading available resident types
/// - Displaying invite success details including OTP
/// </summary>
[Authorize(Roles = "SuperAdmin,Manager")]
public class OnboardingController : Controller
{
    /// <summary>
    /// Service used to communicate with the Onboarding API.
    /// </summary>
    private readonly OnboardingApiService OnboardingApiService;

    /// <summary>
    /// Constructor for injecting OnboardingApiService dependency.
    /// </summary>
    /// <param name="onboardingApiService">
    /// Service responsible for onboarding-related API operations.
    /// </param>
    public OnboardingController(OnboardingApiService onboardingApiService)
    {
        OnboardingApiService = onboardingApiService;
    }

    /// <summary>
    /// Displays the Create Invite page.
    /// </summary>
    /// <returns>
    /// View containing invite creation form with resident types loaded.
    /// </returns>
    [HttpGet]
    public async Task<IActionResult> Create()
    {
        var model = new CreateInviteViewModel();
        await LoadResidentTypesAsync(model);
        return View(model);
    }

    /// <summary>
    /// Handles invite creation request.
    /// </summary>
    /// <param name="model">
    /// CreateInviteViewModel containing resident details.
    /// </param>
    /// <returns>
    /// Success view if invite is created successfully,
    /// otherwise returns form view with validation errors.
    /// </returns>
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

    /// <summary>
    /// Loads available resident types from the API
    /// and populates the CreateInviteViewModel dropdown list.
    /// </summary>
    /// <param name="model">
    /// CreateInviteViewModel that requires resident types to be populated.
    /// </param>
    private async Task LoadResidentTypesAsync(CreateInviteViewModel model)
    {
        var response = await OnboardingApiService.GetResidentTypesAsync();

        model.ResidentTypes = response?.Success == true && response.Data != null
            ? ResidentTypeViewModelMapper.From(response.Data)
            : new List<ResidentTypeOption>();
    }
}




























