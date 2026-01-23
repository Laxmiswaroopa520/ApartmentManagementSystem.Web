
using ApartmentManagementSystem.Web.Mappers.Community;
using ApartmentManagementSystem.Web.Services;
using ApartmentManagementSystem.Web.Services.DTOs;
using ApartmentManagementSystem.Web.Services.DTOs.Common; // ApiResponse<T>
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

    // GET: List All Residents
    [HttpGet]
    public async Task<IActionResult> Index(string? residentType = null)
    {
        var response = string.IsNullOrWhiteSpace(residentType)
            ? await ResidentApiService.GetAllResidentsAsync()
            : await ResidentApiService.GetResidentsByTypeAsync(residentType);

        var viewModel = response?.Success == true && response.Data != null
            ? ResidentListViewModelMapper.From(response.Data)
            : new List<ResidentListViewModel>();

        ViewBag.CurrentFilter = residentType;
        return View(viewModel);
    }

    // GET: Resident Details
    [HttpGet]
    public async Task<IActionResult> Details(Guid userId)
    {
        var response = await ResidentApiService.GetResidentDetailAsync(userId);

        if (response?.Success != true || response.Data == null)
        {
            TempData["ErrorMessage"] = "Resident not found";
            return RedirectToAction(nameof(Index));
        }

        var viewModel = ResidentDetailViewModelMapper.From(response.Data);
        return View(viewModel);
    }
    /*
        // POST: Deactivate Resident
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
        */

    // POST: Deactivate Resident (SuperAdmin & Manager only)
    /* [HttpPost]
     [Authorize(Roles = "SuperAdmin,Manager")]
     public async Task<IActionResult> Deactivate(Guid userId)
     {
         var result = await ResidentApiService.DeactivateResidentAsync(userId);

         if (result?.Success == true)
         {
             TempData["SuccessMessage"] = "Resident deactivated successfully";
         }
         else
         {
             TempData["ErrorMessage"] = result?.Message ?? "Failed to deactivate resident";
         }

         return RedirectToAction(nameof(Index));
     }
     */
    [HttpPost]
    [Authorize(Roles = "SuperAdmin,Manager")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Deactivate(Guid userId)
    {
        var result = await ResidentApiService.DeactivateResidentAsync(userId);

        if (result?.Success == true)
            TempData["SuccessMessage"] = "Resident deactivated successfully";
        else
            TempData["ErrorMessage"] = result?.Message ?? "Failed to deactivate resident";

        return RedirectToAction(nameof(Index));
    }


    // POST: Activate Resident
    [HttpPost]
    [Authorize(Roles = "SuperAdmin,Manager")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Activate(Guid userId)
    {
        await ToggleResidentStatusAsync(
            userId,
            ResidentApiService.ActivateResidentAsync,
            "Resident activated successfully",
            "Failed to activate resident"
        );

        return RedirectToAction(nameof(Index));
    }

    //  PRIVATE HELPER
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


















/*
using ApartmentManagementSystem.Web.Services;
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

    // GET: List All Residents
    [HttpGet]
    public async Task<IActionResult> Index(string? residentType = null)
    {
        var response = string.IsNullOrEmpty(residentType)
            ? await ResidentApiService.GetAllResidentsAsync()
            : await ResidentApiService.GetResidentsByTypeAsync(residentType);

        var viewModel = response?.Data?
            .Select(r => new ResidentListViewModel
            {
                UserId = r.UserId,
                FullName = r.FullName,
                Email = r.Email,
                Phone = r.Phone,
                ResidentType = r.ResidentType,
                FlatNumber = r.FlatNumber,
                Status = r.Status,
                RegisteredOn = r.RegisteredOn
            })
            .ToList() ?? new List<ResidentListViewModel>();

        ViewBag.CurrentFilter = residentType;
        return View(viewModel);
    }

    // GET: Resident Details
    [HttpGet]
    public async Task<IActionResult> Details(Guid userId)
    {
        var response = await ResidentApiService.GetResidentDetailAsync(userId);

        if (response?.Data == null)
        {
            TempData["ErrorMessage"] = "Resident not found";
            return RedirectToAction(nameof(Index));
        }

        var viewModel = new ResidentDetailViewModel
        {
            UserId = response.Data.UserId,
            FullName = response.Data.FullName,
            Email = response.Data.Email,
            PrimaryPhone = response.Data.PrimaryPhone,
            SecondaryPhone = response.Data.SecondaryPhone,
            ResidentType = response.Data.ResidentType,
            FlatNumber = response.Data.FlatNumber,
            ApartmentName = response.Data.ApartmentName,
            RegisteredOn = response.Data.RegisteredOn,
            Status = response.Data.Status,
            Roles = response.Data.Roles,
            TotalComplaints = response.Data.TotalComplaints,
            OutstandingBills = response.Data.OutstandingBills
        };

        return View(viewModel);
    }

    // POST: Deactivate Resident (SuperAdmin & Manager only)
    [HttpPost]
    [Authorize(Roles = "SuperAdmin,Manager")]
    public async Task<IActionResult> Deactivate(Guid userId)
    {
        var result = await ResidentApiService.DeactivateResidentAsync(userId);

        if (result?.Success == true)
        {
            TempData["SuccessMessage"] = "Resident deactivated successfully";
        }
        else
        {
            TempData["ErrorMessage"] = result?.Message ?? "Failed to deactivate resident";
        }

        return RedirectToAction(nameof(Index));
    }

    // POST: Activate Resident (SuperAdmin & Manager only)
    [HttpPost]
    [Authorize(Roles = "SuperAdmin,Manager")]
    public async Task<IActionResult> Activate(Guid userId)
    {
        var result = await ResidentApiService.ActivateResidentAsync(userId);

        if (result?.Success == true)
        {
            TempData["SuccessMessage"] = "Resident activated successfully";
        }
        else
        {
            TempData["ErrorMessage"] = result?.Message ?? "Failed to activate resident";
        }

        return RedirectToAction(nameof(Index));
    }
}
*/
