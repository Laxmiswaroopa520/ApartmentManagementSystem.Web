

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ApartmentManagementSystem.Web.Services;
using ApartmentManagementSystem.Web.ViewModels.Community;

namespace ApartmentManagementSystem.Web.Controllers
{
    [Authorize(Roles = "SuperAdmin,Manager")]
    public class CommunityMembersController : Controller
    {
        private readonly CommunityMemberApiService _communityService;

        public CommunityMembersController(CommunityMemberApiService communityService)
        {
            _communityService = communityService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            try
            {
                var response = await _communityService.GetAllCommunityMembersAsync();

                var viewModel = response?.Success == true && response.Data != null
                    ? response.Data.Select(dto => new CommunityMemberViewModel
                    {
                        UserId = dto.UserId,
                        FullName = dto.FullName,
                        Role = dto.Role,
                        FlatNumber = dto.FlatNumber,
                        Email = dto.Email,
                        Phone = dto.Phone,
                        AssignedOn = dto.AssignedOn,
                        IsActive = dto.IsActive
                    }).ToList()
                    : new List<CommunityMemberViewModel>();

                return View(viewModel);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading community members: {ex.Message}");
                TempData["ErrorMessage"] = "Failed to load community members";
                return View(new List<CommunityMemberViewModel>());
            }
        }

        [HttpGet]
        public async Task<IActionResult> AssignRole(Guid? apartmentId, string? role)
        {
            try
            {
                var response = await _communityService.GetEligibleResidentsAsync();

                ViewBag.EligibleResidents = response?.Success == true && response.Data != null
                    ? response.Data
                    : new List<Services.DTOs.Community.ResidentListDto>();

                var viewModel = new AssignCommunityRoleViewModel
                {
                    ApartmentId = apartmentId,
                    CommunityRole = role
                };

                return View(viewModel);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading eligible residents: {ex.Message}");
                TempData["ErrorMessage"] = "Failed to load eligible residents";
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AssignRole(AssignCommunityRoleViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var response = await _communityService.GetEligibleResidentsAsync();
                ViewBag.EligibleResidents = response?.Success == true && response.Data != null
                    ? response.Data
                    : new List<Services.DTOs.Community.ResidentListDto>();

                return View(model);
            }

            try
            {
                var request = new Services.DTOs.Community.AssignCommunityRoleRequest
                {
                    UserId = model.UserId,
                    CommunityRole = model.CommunityRole
                };

                var result = await _communityService.AssignCommunityRoleAsync(request);

                if (result?.Success == true)
                {
                    TempData["SuccessMessage"] = $"Successfully assigned {model.CommunityRole} role!";

                    // If came from apartment details, redirect back there
                    if (model.ApartmentId.HasValue)
                    {
                        return RedirectToAction("Details", "ApartmentBuilder", new { id = model.ApartmentId.Value });
                    }

                    return RedirectToAction(nameof(Index));
                }

                TempData["ErrorMessage"] = result?.Message ?? "Failed to assign role";
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error assigning role: {ex.Message}");
                TempData["ErrorMessage"] = "An error occurred while assigning the role";
            }

            var residentsResponse = await _communityService.GetEligibleResidentsAsync();
            ViewBag.EligibleResidents = residentsResponse?.Success == true && residentsResponse.Data != null
                ? residentsResponse.Data
                : new List<Services.DTOs.Community.ResidentListDto>();

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveRole(Guid userId)
        {
            try
            {
                var request = new Services.DTOs.Community.RemoveCommunityRoleRequest
                {
                    UserId = userId
                };

                var result = await _communityService.RemoveCommunityRoleAsync(request);

                if (result?.Success == true)
                {
                    TempData["SuccessMessage"] = "Role removed successfully!";
                }
                else
                {
                    TempData["ErrorMessage"] = result?.Message ?? "Failed to remove role";
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error removing role: {ex.Message}");
                TempData["ErrorMessage"] = "An error occurred while removing the role";
            }

            return RedirectToAction(nameof(Index));
        }
    }
}





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














































