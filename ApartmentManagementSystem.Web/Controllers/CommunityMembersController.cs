





/*using ApartmentManagementSystem.Web.Mappers.Community;
using ApartmentManagementSystem.Web.Services;
using ApartmentManagementSystem.Web.Services.DTOs.Community;
using ApartmentManagementSystem.Web.ViewModels.Community;
using ApartmentManagementSystem.Web.ViewModels.Dashboard;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ApartmentManagementSystem.Web.Controllers;

[Authorize(Roles = "SuperAdmin,Manager")]
public class CommunityMembersController : Controller
{
    private readonly CommunityMemberApiService CommunityApiService;

    public CommunityMembersController(CommunityMemberApiService communityApiService)
    {
        CommunityApiService = communityApiService;
    }

    // GET: Community Members
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var response = await CommunityApiService.GetAllCommunityMembersAsync();

        var viewModel = response?.Success == true && response.Data != null
            ? CommunityMemberViewModelMapper.From(response.Data)
            : new List<CommunityMemberViewModel>();

        return View(viewModel);
    }

    // GET: Assign Role
    [HttpGet]
    public async Task<IActionResult> AssignRole()
    {
        await LoadEligibleResidentsAsync();
        return View(new AssignCommunityRoleViewModel());
    }

    // POST: Assign Role
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AssignRole(AssignCommunityRoleViewModel model)
    {
        if (!ModelState.IsValid)
        {
            await LoadEligibleResidentsAsync();
            return View(model);
        }

        var request = new AssignCommunityRoleRequest
        {
            UserId = model.UserId,
            CommunityRole = model.CommunityRole
        };

        var result = await CommunityApiService.AssignCommunityRoleAsync(request);

        if (result?.Success == true)
        {
            TempData["SuccessMessage"] = $"{model.CommunityRole} assigned successfully";
            return RedirectToAction(nameof(Index));
        }

        TempData["ErrorMessage"] = result?.Message ?? "Failed to assign role";
        await LoadEligibleResidentsAsync();
        return View(model);
    }

    // POST: Remove Role
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> RemoveRole(Guid userId)
    {
        var request = new RemoveCommunityRoleRequest { UserId = userId };

        var result = await CommunityApiService.RemoveCommunityRoleAsync(request);

        if (result?.Success == true)
            TempData["SuccessMessage"] = "Community role removed successfully";
        else
            TempData["ErrorMessage"] = result?.Message ?? "Failed to remove role";

        return RedirectToAction(nameof(Index));
    }

    //private helper method::It loads and prepares the dropdown data for “Assign Community Role” pages:Avoids code duplication
    private async Task LoadEligibleResidentsAsync()
    {
        var response = await CommunityApiService.GetEligibleResidentsAsync();

        ViewBag.EligibleResidents = response?.Success == true && response.Data != null
            ? EligibleResidentViewModelMapper.From(response.Data)
            : new List<EligibleResidentViewModel>();
    }
}

*/














































