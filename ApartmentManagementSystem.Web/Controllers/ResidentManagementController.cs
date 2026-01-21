using ApartmentManagementSystem.Web.Services;
using ApartmentManagementSystem.Web.ViewModels.Community;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ApartmentManagementSystem.Web.Controllers;

[Authorize(Roles = "SuperAdmin,Manager,President,Secretary,Treasurer")]
public class ResidentManagementController : Controller
{
    private readonly ResidentManagementApiService _residentApiService;

    public ResidentManagementController(ResidentManagementApiService residentApiService)
    {
        _residentApiService = residentApiService;
    }

    // =========================
    // GET: List All Residents
    // =========================
    [HttpGet]
    public async Task<IActionResult> Index(string? residentType = null)
    {
        var response = string.IsNullOrEmpty(residentType)
            ? await _residentApiService.GetAllResidentsAsync()
            : await _residentApiService.GetResidentsByTypeAsync(residentType);

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

    // =========================
    // GET: Resident Details
    // =========================
    [HttpGet]
    public async Task<IActionResult> Details(Guid userId)
    {
        var response = await _residentApiService.GetResidentDetailAsync(userId);

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

    // =========================
    // POST: Deactivate Resident (SuperAdmin & Manager only)
    // =========================
    [HttpPost]
    [Authorize(Roles = "SuperAdmin,Manager")]
    public async Task<IActionResult> Deactivate(Guid userId)
    {
        var result = await _residentApiService.DeactivateResidentAsync(userId);

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

    // =========================
    // POST: Activate Resident (SuperAdmin & Manager only)
    // =========================
    [HttpPost]
    [Authorize(Roles = "SuperAdmin,Manager")]
    public async Task<IActionResult> Activate(Guid userId)
    {
        var result = await _residentApiService.ActivateResidentAsync(userId);

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