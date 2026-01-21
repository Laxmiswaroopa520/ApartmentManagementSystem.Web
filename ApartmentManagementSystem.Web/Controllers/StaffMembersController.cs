using ApartmentManagementSystem.Web.ViewModels.Community;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ApartmentManagementSystem.Web.Services.DTOs.Staff;
using ApartmentManagementSystem.Web.Services;

namespace ApartmentManagementSystem.Web.Controllers;

[Authorize(Roles = "SuperAdmin,Manager,President,Secretary,Treasurer")]
public class StaffMembersController : Controller
{
    private readonly StaffMemberApiService _staffApiService;

    public StaffMembersController(StaffMemberApiService staffApiService)
    {
        _staffApiService = staffApiService;
    }

    // =========================
    // GET: List All Staff Members
    // =========================
    [HttpGet]
    public async Task<IActionResult> Index(string? staffType = null)
    {
        var response = string.IsNullOrEmpty(staffType)
            ? await _staffApiService.GetAllStaffMembersAsync()
            : await _staffApiService.GetStaffMembersByTypeAsync(staffType);

        var viewModel = response?.Data?
            .Select(s => new StaffMemberViewModel
            {
                StaffId = s.StaffId,
                FullName = s.FullName,
                Phone = s.Phone,
                Email = s.Email,
                Address = s.Address,
                StaffType = s.StaffType,
                JoinedOn = s.JoinedOn,
                IsActive = s.IsActive,
                Specialization = s.Specialization,
                HourlyRate = s.HourlyRate
            })
            .ToList() ?? new List<StaffMemberViewModel>();

        ViewBag.CurrentFilter = staffType;
        return View(viewModel);
    }

    // =========================
    // GET: Create Staff Member
    // =========================
    [HttpGet]
    public IActionResult Create()
    {
        return View(new CreateStaffMemberViewModel());
    }

    // =========================
    // POST: Create Staff Member
    // =========================
    [HttpPost]
    public async Task<IActionResult> Create(CreateStaffMemberViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var request = new CreateStaffMemberRequest
        {
            FullName = model.FullName,
            Phone = model.Phone,
            Email = model.Email,
            Address = model.Address,
            StaffType = model.StaffType,
            Specialization = model.Specialization,
            HourlyRate = model.HourlyRate,
            Password = model.CreateLoginAccess ? model.Password : null
        };

        var result = await _staffApiService.CreateStaffMemberAsync(request);

        if (result?.Success == true)
        {
            TempData["SuccessMessage"] = "Staff member created successfully";
            return RedirectToAction(nameof(Index));
        }

        ModelState.AddModelError("", result?.Message ?? "Failed to create staff member");
        return View(model);
    }

    // =========================
    // GET: Edit Staff Member
    // =========================
    [HttpGet]
    public async Task<IActionResult> Edit(Guid staffId)
    {
        var response = await _staffApiService.GetStaffMemberByIdAsync(staffId);

        if (response?.Data == null)
        {
            TempData["ErrorMessage"] = "Staff member not found";
            return RedirectToAction(nameof(Index));
        }

        var viewModel = new UpdateStaffMemberViewModel
        {
            StaffId = response.Data.StaffId,
            FullName = response.Data.FullName,
            Phone = response.Data.Phone,
            Email = response.Data.Email,
            Address = response.Data.Address,
            IsActive = response.Data.IsActive,
            Specialization = response.Data.Specialization,
            HourlyRate = response.Data.HourlyRate
        };

        return View(viewModel);
    }

    // =========================
    // POST: Edit Staff Member
    // =========================
    [HttpPost]
    public async Task<IActionResult> Edit(UpdateStaffMemberViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var request = new UpdateStaffMemberRequest
        {
            StaffId = model.StaffId,
            FullName = model.FullName,
            Phone = model.Phone,
            Email = model.Email,
            Address = model.Address,
            IsActive = model.IsActive,
            Specialization = model.Specialization,
            HourlyRate = model.HourlyRate
        };

        var result = await _staffApiService.UpdateStaffMemberAsync(request);

        if (result?.Success == true)
        {
            TempData["SuccessMessage"] = "Staff member updated successfully";
            return RedirectToAction(nameof(Index));
        }

        ModelState.AddModelError("", result?.Message ?? "Failed to update staff member");
        return View(model);
    }

    // =========================
    // POST: Deactivate Staff Member
    // =========================
    [HttpPost]
    public async Task<IActionResult> Deactivate(Guid staffId)
    {
        var result = await _staffApiService.DeactivateStaffMemberAsync(staffId);

        if (result?.Success == true)
        {
            TempData["SuccessMessage"] = "Staff member deactivated successfully";
        }
        else
        {
            TempData["ErrorMessage"] = result?.Message ?? "Failed to deactivate staff member";
        }

        return RedirectToAction(nameof(Index));
    }

    // =========================
    // POST: Activate Staff Member
    // =========================
    [HttpPost]
    public async Task<IActionResult> Activate(Guid staffId)
    {
        var result = await _staffApiService.ActivateStaffMemberAsync(staffId);

        if (result?.Success == true)
        {
            TempData["SuccessMessage"] = "Staff member activated successfully";
        }
        else
        {
            TempData["ErrorMessage"] = result?.Message ?? "Failed to activate staff member";
        }

        return RedirectToAction(nameof(Index));
    }
}
