using ApartmentManagementSystem.Web.Mappers.Community;
using ApartmentManagementSystem.Web.Services;
using ApartmentManagementSystem.Web.Services.DTOs;
using ApartmentManagementSystem.Web.ViewModels.Community;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ApartmentManagementSystem.Web.Controllers;

[Authorize(Roles = "SuperAdmin,Manager,President,Secretary,Treasurer")]
public class ResidentManagementController : Controller
{
    private readonly ResidentManagementApiService ResidentApiService;

    public ResidentManagementController(ResidentManagementApiService residentApiService)
    {
        ResidentApiService = residentApiService;
    }

    [HttpGet]
    public async Task<IActionResult> Index(string? residentType = null)
    {
        var response = string.IsNullOrWhiteSpace(residentType)
            ? await ResidentApiService.GetAllResidentsAsync()
            : await ResidentApiService.GetResidentsByTypeAsync(residentType);

        var viewModel = response.Success && response.Data != null
            ? ResidentListViewModelMapper.From(response.Data)
            : new List<ResidentListViewModel>();

        ViewBag.CurrentFilter = residentType;
        return View(viewModel);
    }

    [HttpGet]
    public async Task<IActionResult> Details(Guid userId)
    {
        var response = await ResidentApiService.GetResidentDetailAsync(userId);

        if (!response.Success || response.Data == null)
        {
            TempData["ErrorMessage"] = "Resident not found";
            return RedirectToAction(nameof(Index));
        }

        return View(ResidentDetailViewModelMapper.From(response.Data));
    }

    [HttpPost]
    [Authorize(Roles = "SuperAdmin,Manager")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Deactivate(Guid userId)
    {
        await ToggleResidentStatusAsync(
            userId,
            ResidentApiService.DeactivateResidentAsync,
            "Resident deactivated successfully",
            "Failed to deactivate resident"
        );


        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [Authorize(Roles = "SuperAdmin,Manager")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Activate(Guid userId)
    {
        await ToggleResidentStatusAsync(
     userId,
     id => ResidentApiService.ActivateResidentAsync(id),
     "Resident activated successfully",
     "Failed to activate resident"
 );



        return RedirectToAction(nameof(Index));
    }

    private async Task ToggleResidentStatusAsync(
        Guid userId,
        Func<Guid, Task<ApiResponse<bool>>> action,
        string successMessage,
        string failureMessage)
    {
        var result = await action(userId);

        if (result.Success)
            TempData["SuccessMessage"] = successMessage;
        else
            TempData["ErrorMessage"] = result.Message ?? failureMessage;
    }
}















