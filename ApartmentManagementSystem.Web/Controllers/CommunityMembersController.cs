using ApartmentManagementSystem.Web.Services;
using ApartmentManagementSystem.Web.ViewModels.Community;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ApartmentManagementSystem.Web.Controllers;

[Authorize(Roles = "SuperAdmin,Manager")]
public class CommunityMembersController : Controller
{
    private readonly CommunityMemberApiService _communityApiService;

    public CommunityMembersController(CommunityMemberApiService communityApiService)
    {
        _communityApiService = communityApiService;
    }

    // =========================
    // GET: List All Community Members
    // =========================
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var response = await _communityApiService.GetAllCommunityMembersAsync();

        var viewModel = response?.Data?
            .Select(m => new CommunityMemberViewModel
            {
                UserId = m.UserId,
                FullName = m.FullName,
                Email = m.Email,
                Phone = m.Phone,
                FlatNumber = m.FlatNumber,
                Role = m.Role,
                AssignedOn = m.AssignedOn,
                IsActive = m.IsActive
            })
            .ToList() ?? new List<CommunityMemberViewModel>();

        return View(viewModel);
    }

    // =========================
    // GET: Assign Community Role
    // =========================
    [HttpGet]
    public async Task<IActionResult> AssignRole()
    {
        var response = await _communityApiService.GetEligibleResidentsAsync();

        ViewBag.EligibleResidents = response?.Data?
            .Select(r => new
            {
                UserId = r.UserId,
                DisplayText = $"{r.FullName} - Flat {r.FlatNumber}"
            })
            .ToList() ?? new List<dynamic>();

        return View(new AssignCommunityRoleViewModel());
    }

    // =========================
    // POST: Assign Community Role
    // =========================
    [HttpPost]
    public async Task<IActionResult> AssignRole(AssignCommunityRoleViewModel model)
    {
        if (!ModelState.IsValid)
        {
            var response = await _communityApiService.GetEligibleResidentsAsync();
            ViewBag.EligibleResidents = response?.Data?
                .Select(r => new
                {
                    UserId = r.UserId,
                    DisplayText = $"{r.FullName} - Flat {r.FlatNumber}"
                })
                .ToList() ?? new List<dynamic>();

            return View(model);
        }

        var request = new AssignCommunityRoleRequest
        {
            UserId = model.UserId,
            CommunityRole = model.CommunityRole
        };

        var result = await _communityApiService.AssignCommunityRoleAsync(request);

        if (result?.Success == true)
        {
            TempData["SuccessMessage"] = $"{model.CommunityRole} assigned successfully";
            return RedirectToAction(nameof(Index));
        }

        ModelState.AddModelError("", result?.Message ?? "Failed to assign role");
        return View(model);
    }

    // =========================
    // POST: Remove Community Role
    // =========================
    [HttpPost]
    public async Task<IActionResult> RemoveRole(Guid userId)
    {
        var request = new RemoveCommunityRoleRequest { UserId = userId };
        var result = await _communityApiService.RemoveCommunityRoleAsync(request);

        if (result?.Success == true)
        {
            TempData["SuccessMessage"] = "Community role removed successfully";
        }
        else
        {
            TempData["ErrorMessage"] = result?.Message ?? "Failed to remove role";
        }

        return RedirectToAction(nameof(Index));
    }
}

// ========================================
// REQUEST DTOs (for API service)
// ========================================
public class AssignCommunityRoleRequest
{
    public Guid UserId { get; set; }
    public string CommunityRole { get; set; } = string.Empty;
}

public class RemoveCommunityRoleRequest
{
    public Guid UserId { get; set; }
}