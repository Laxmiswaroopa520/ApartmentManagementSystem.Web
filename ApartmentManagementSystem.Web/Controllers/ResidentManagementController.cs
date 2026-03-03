using ApartmentManagementSystem.Web.Mappers.Community;
using ApartmentManagementSystem.Web.Services;
using ApartmentManagementSystem.Web.Services.DTOs;
using ApartmentManagementSystem.Web.ViewModels.Community;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ApartmentManagementSystem.Web.Controllers;

/// <summary>
/// Controller responsible for managing residents within the system.
/// 
/// Accessible by:
/// SuperAdmin, Manager, President, Secretary, Treasurer.
/// 
/// Responsibilities:
/// - Viewing all residents
/// - Filtering residents by type
/// - Viewing resident details
/// - Activating and deactivating residents (restricted to SuperAdmin & Manager)
/// </summary>
[Authorize(Roles = "SuperAdmin,Manager,President,Secretary,Treasurer")]
public class ResidentManagementController : Controller
{
    /// <summary>
    /// Service used to communicate with Resident Management API.
    /// </summary>
    private readonly ResidentManagementApiService ResidentApiService;

    /// <summary>
    /// Constructor for injecting ResidentManagementApiService dependency.
    /// </summary>
    /// <param name="residentApiService">
    /// Service responsible for resident-related API operations.
    /// </param>
    public ResidentManagementController(ResidentManagementApiService residentApiService)
    {
        ResidentApiService = residentApiService;
    }

    /// <summary>
    /// Displays list of residents.
    /// Optionally filters residents by resident type.
    /// </summary>
    /// <param name="residentType">
    /// Optional resident type filter (e.g., Owner, Tenant).
    /// </param>
    /// <returns>
    /// View containing list of residents.
    /// </returns>
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

    /// <summary>
    /// Displays detailed information about a specific resident.
    /// </summary>
    /// <param name="userId">
    /// Unique identifier of the resident.
    /// </param>
    /// <returns>
    /// Resident details view or redirects to Index if not found.
    /// </returns>
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

    /// <summary>
    /// Deactivates a resident account.
    /// Accessible only by SuperAdmin and Manager.
    /// </summary>
    /// <param name="userId">
    /// Unique identifier of the resident.
    /// </param>
    /// <returns>
    /// Redirects to Index with success or failure message.
    /// </returns>
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

    /// <summary>
    /// Activates a resident account.
    /// Accessible only by SuperAdmin and Manager.
    /// </summary>
    /// <param name="userId">
    /// Unique identifier of the resident.
    /// </param>
    /// <returns>
    /// Redirects to Index with success or failure message.
    /// </returns>
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

    /// <summary>
    /// Helper method to toggle resident status (activate/deactivate).
    /// Executes provided action and sets appropriate TempData message.
    /// </summary>
    /// <param name="userId">
    /// Unique identifier of the resident.
    /// </param>
    /// <param name="action">
    /// API action to execute (activate or deactivate).
    /// </param>
    /// <param name="successMessage">
    /// Message displayed on successful operation.
    /// </param>
    /// <param name="failureMessage">
    /// Default message displayed on failure.
    /// </param>
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









/*using ApartmentManagementSystem.Web.Mappers.Community;
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
*/















