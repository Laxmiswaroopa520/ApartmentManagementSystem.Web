using ApartmentManagementSystem.Web.Constants;
using ApartmentManagementSystem.Web.Services;
using ApartmentManagementSystem.Web.Services.DTOs.Staff;
using ApartmentManagementSystem.Web.ViewModels.Community;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ApartmentManagementSystem.Web.Controllers;

/// <summary>
/// Handles all staff member management operations for the web layer.
/// Accessible to SuperAdmin, Manager, President, Secretary, and Treasurer roles.
/// Responsibilities include listing, creating, editing, activating, and deactivating staff members,
/// as well as assigning them to specific apartments.
/// </summary>
[Authorize(Roles = AppRoles.AdminManagerCommunity)]
public class StaffMembersController : Controller
{
    private readonly StaffMemberApiService StaffApiService;
    private readonly ApartmentApiService ApartmentApiService;

    /// <summary>
    /// Injects the staff member API service and apartment API service
    /// required for staff CRUD operations and apartment dropdown population.
    /// </summary>
    public StaffMembersController(
        StaffMemberApiService staffApiService,
        ApartmentApiService apartmentApiService)
    {
        StaffApiService = staffApiService;
        ApartmentApiService = apartmentApiService;
    }

    /// <summary>
    /// Displays the list of all staff members, optionally filtered by staff type
    /// (e.g., Security, Plumber, Electrician).
    /// Each staff member row includes the apartment they are assigned to.
    /// </summary>
    /// <param name="staffType">
    /// Optional filter. When provided, only staff of that type are shown.
    /// When null or empty, all staff members are returned.
    /// </param>
    [HttpGet]
    public async Task<IActionResult> Index(string? staffType = null)
    {
        var response = string.IsNullOrEmpty(staffType)
            ? await StaffApiService.GetAllStaffMembersAsync()
            : await StaffApiService.GetStaffMembersByTypeAsync(staffType);

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
                HourlyRate = s.HourlyRate,
                ApartmentId = s.ApartmentId,
                ApartmentName = s.ApartmentName
            }).ToList() ?? new List<StaffMemberViewModel>();

        ViewBag.CurrentFilter = staffType;
        return View(viewModel);
    }

    /// <summary>
    /// Renders the Create Staff Member form.
    /// Populates the apartment dropdown so the user can assign the new staff member
    /// to a specific apartment during creation.
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> Create()
    {
        await PopulateApartmentsAsync();
        return View(new CreateStaffMemberViewModel());
    }

    /// <summary>
    /// Processes the Create Staff Member form submission.
    /// Validates the model, maps it to a request DTO, and calls the API.
    /// On success, redirects to the Index page with a success message.
    /// On failure, re-renders the form with validation errors and repopulates the apartment dropdown.
    /// </summary>
    /// <param name="model">The form data submitted by the user.</param>
    [HttpPost]
    public async Task<IActionResult> Create(CreateStaffMemberViewModel model)
    {
        if (!ModelState.IsValid)
        {
            await PopulateApartmentsAsync();
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
            Password = model.CreateLoginAccess ? model.Password : null,
            ApartmentId = model.ApartmentId
        };

        var result = await StaffApiService.CreateStaffMemberAsync(request);

        if (result?.Success == true)
        {
            TempData[AppMessages.SuccessMessage] = "Staff member created successfully";
            return RedirectToAction(nameof(Index));
        }

        ModelState.AddModelError(string.Empty, result?.Message ?? "Failed to create staff member");
        await PopulateApartmentsAsync();
        return View(model);
    }

    /// <summary>
    /// Renders the Edit Staff Member form pre-filled with the existing staff member data.
    /// Also populates the apartment dropdown with the currently assigned apartment pre-selected.
    /// Redirects to Index if the staff member is not found.
    /// </summary>
    /// <param name="staffId">The unique identifier of the staff member to edit.</param>
    [HttpGet]
    public async Task<IActionResult> Edit(Guid staffId)
    {
        var response = await StaffApiService.GetStaffMemberByIdAsync(staffId);

        if (response?.Data == null)
        {
            TempData[AppMessages.ErrorMessage] = "Staff member not found";
            return RedirectToAction(nameof(Index));
        }

        var vm = new UpdateStaffMemberViewModel
        {
            StaffId = response.Data.StaffId,
            FullName = response.Data.FullName,
            Phone = response.Data.Phone,
            Email = response.Data.Email,
            Address = response.Data.Address,
            IsActive = response.Data.IsActive,
            Specialization = response.Data.Specialization,
            HourlyRate = response.Data.HourlyRate,
            ApartmentId = response.Data.ApartmentId,
            ApartmentName = response.Data.ApartmentName
        };

        await PopulateApartmentsAsync();
        return View(vm);
    }

    /// <summary>
    /// Processes the Edit Staff Member form submission.
    /// Validates the model, maps it to an update request DTO, and calls the API.
    /// On success, redirects to the Index page with a success message.
    /// On failure, re-renders the form with errors and repopulates the apartment dropdown.
    /// </summary>
    /// <param name="model">The updated staff member data submitted by the user.</param>
    [HttpPost]
    public async Task<IActionResult> Edit(UpdateStaffMemberViewModel model)
    {
        if (!ModelState.IsValid)
        {
            await PopulateApartmentsAsync();
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
            HourlyRate = model.HourlyRate,
            ApartmentId = model.ApartmentId
        };

        var result = await StaffApiService.UpdateStaffMemberAsync(request);

        if (result?.Success == true)
        {
            TempData[AppMessages.SuccessMessage] = "Staff member updated successfully";
            return RedirectToAction(nameof(Index));
        }

        ModelState.AddModelError(string.Empty, result?.Message ?? "Failed to update staff member");
        await PopulateApartmentsAsync();
        return View(model);
    }

    /// <summary>
    /// Deactivates a staff member, preventing them from being assigned tasks
    /// or accessing the system (if they have login access).
    /// Sets an appropriate TempData message and redirects to Index.
    /// </summary>
    /// <param name="staffId">The unique identifier of the staff member to deactivate.</param>
    [HttpPost]
    public async Task<IActionResult> Deactivate(Guid staffId)
    {
        var result = await StaffApiService.DeactivateStaffMemberAsync(staffId);
        TempData[result?.Success == true ? AppMessages.SuccessMessage : AppMessages.ErrorMessage] =
            result?.Success == true
                ? "Staff member deactivated successfully"
                : result?.Message ?? "Failed to deactivate staff member";
        return RedirectToAction(nameof(Index));
    }

    /// <summary>
    /// Reactivates a previously deactivated staff member,
    /// restoring their active status in the system.
    /// Sets an appropriate TempData message and redirects to Index.
    /// </summary>
    /// <param name="staffId">The unique identifier of the staff member to activate.</param>
    [HttpPost]
    public async Task<IActionResult> Activate(Guid staffId)
    {
        var result = await StaffApiService.ActivateStaffMemberAsync(staffId);
        TempData[result?.Success == true ? AppMessages.SuccessMessage : AppMessages.ErrorMessage] =
            result?.Success == true
                ? "Staff member activated successfully"
                : result?.Message ?? "Failed to activate staff member";
        return RedirectToAction(nameof(Index));
    }

    /// <summary>
    /// Fetches all apartments from the API and stores them as a typed list in ViewBag.Apartments.
    /// Used to populate the apartment assignment dropdown on the Create and Edit forms.
    /// Falls back to an empty list on failure so the form still renders without crashing.
    /// </summary>
    private async Task PopulateApartmentsAsync()
    {
        try
        {
            var response = await ApartmentApiService.GetAllApartmentsAsync();
            ViewBag.Apartments = response?.Success == true && response.Data != null
                ? response.Data.Select(a => new ApartmentDropdownItem { Id = a.Id, Name = a.Name }).ToList()
                : new List<ApartmentDropdownItem>();
        }
        catch
        {
            ViewBag.Apartments = new List<ApartmentDropdownItem>();
        }
    }
}




























/*using ApartmentManagementSystem.Web.Constants;
using ApartmentManagementSystem.Web.Services;
using ApartmentManagementSystem.Web.Services.DTOs.Staff;
using ApartmentManagementSystem.Web.ViewModels.Community;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ApartmentManagementSystem.Web.Controllers;

[Authorize(Roles = AppRoles.AdminManagerCommunity)]
public class StaffMembersController : Controller
{
    private readonly StaffMemberApiService StaffApiService;
    private readonly ApartmentApiService ApartmentApiService;

    public StaffMembersController(
        StaffMemberApiService staffApiService,
        ApartmentApiService apartmentApiService)
    {
        StaffApiService = staffApiService;
        ApartmentApiService = apartmentApiService;
    }

    [HttpGet]
    public async Task<IActionResult> Index(string? staffType = null)
    {
        var response = string.IsNullOrEmpty(staffType)
            ? await StaffApiService.GetAllStaffMembersAsync()
            : await StaffApiService.GetStaffMembersByTypeAsync(staffType);

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
                HourlyRate = s.HourlyRate,
                ApartmentId = s.ApartmentId,
                ApartmentName = s.ApartmentName
            }).ToList() ?? new List<StaffMemberViewModel>();

        ViewBag.CurrentFilter = staffType;
        return View(viewModel);
    }

    [HttpGet]
    public async Task<IActionResult> Create()
    {
        await PopulateApartmentsAsync();
        return View(new CreateStaffMemberViewModel());
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateStaffMemberViewModel model)
    {
        if (!ModelState.IsValid)
        {
            await PopulateApartmentsAsync();
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
            Password = model.CreateLoginAccess ? model.Password : null,
            ApartmentId = model.ApartmentId
        };

        var result = await StaffApiService.CreateStaffMemberAsync(request);

        if (result?.Success == true)
        {
            TempData[AppMessages.SuccessMessage] = "Staff member created successfully";
            return RedirectToAction(nameof(Index));
        }

        ModelState.AddModelError(string.Empty, result?.Message ?? "Failed to create staff member");
        await PopulateApartmentsAsync();
        return View(model);
    }

    [HttpGet]
    public async Task<IActionResult> Edit(Guid staffId)
    {
        var response = await StaffApiService.GetStaffMemberByIdAsync(staffId);

        if (response?.Data == null)
        {
            TempData[AppMessages.ErrorMessage] = "Staff member not found";
            return RedirectToAction(nameof(Index));
        }

        var vm = new UpdateStaffMemberViewModel
        {
            StaffId = response.Data.StaffId,
            FullName = response.Data.FullName,
            Phone = response.Data.Phone,
            Email = response.Data.Email,
            Address = response.Data.Address,
            IsActive = response.Data.IsActive,
            Specialization = response.Data.Specialization,
            HourlyRate = response.Data.HourlyRate,
            ApartmentId = response.Data.ApartmentId,
            ApartmentName = response.Data.ApartmentName
        };

        await PopulateApartmentsAsync();
        return View(vm);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(UpdateStaffMemberViewModel model)
    {
        if (!ModelState.IsValid)
        {
            await PopulateApartmentsAsync();
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
            HourlyRate = model.HourlyRate,
            ApartmentId = model.ApartmentId
        };

        var result = await StaffApiService.UpdateStaffMemberAsync(request);

        if (result?.Success == true)
        {
            TempData[AppMessages.SuccessMessage] = "Staff member updated successfully";
            return RedirectToAction(nameof(Index));
        }

        ModelState.AddModelError(string.Empty, result?.Message ?? "Failed to update staff member");
        await PopulateApartmentsAsync();
        return View(model);
    }

    // DEACTIVATE 
    [HttpPost]
    public async Task<IActionResult> Deactivate(Guid staffId)
    {
        var result = await StaffApiService.DeactivateStaffMemberAsync(staffId);
        TempData[result?.Success == true ? AppMessages.SuccessMessage : AppMessages.ErrorMessage] =
            result?.Success == true ? "Staff member deactivated successfully"
                                    : result?.Message ?? "Failed to deactivate staff member";
        return RedirectToAction(nameof(Index));
    }

    //ACTIVATE 
    [HttpPost]
    public async Task<IActionResult> Activate(Guid staffId)
    {
        var result = await StaffApiService.ActivateStaffMemberAsync(staffId);
        TempData[result?.Success == true ? AppMessages.SuccessMessage : AppMessages.ErrorMessage] =
            result?.Success == true ? "Staff member activated successfully"
                                    : result?.Message ?? "Failed to activate staff member";
        return RedirectToAction(nameof(Index));
    }

    // Helper Method: Populate apartment dropdown via ViewBag 
    private async Task PopulateApartmentsAsync()
    {
        try
        {
            var response = await ApartmentApiService.GetAllApartmentsAsync();
            if (response?.Success == true && response.Data != null)
            {
                ViewBag.Apartments = response.Data
                    .Select(a => new ApartmentDropdownItem { Id = a.Id, Name = a.Name })
                    .ToList();
            }
            else
            {
                ViewBag.Apartments = new List<ApartmentDropdownItem>();
            }
        }
        catch
        {
            ViewBag.Apartments = new List<ApartmentDropdownItem>();
        }
    }
}
*/









