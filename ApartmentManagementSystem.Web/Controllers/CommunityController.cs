// Web/Controllers/CommunityController.cs
// COMPLETE REPLACEMENT — scopes everything to a specific apartment

using ApartmentManagementSystem.Web.Mappers.Community;
using ApartmentManagementSystem.Web.Services;
using ApartmentManagementSystem.Web.Services.DTOs.Community;
using ApartmentManagementSystem.Web.ViewModels.Community;
using ApartmentManagementSystem.Web.ViewModels.Dashboard;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ApartmentManagementSystem.Web.Controllers
{
    [Authorize(Roles = "SuperAdmin,Manager,President,Secretary,Treasurer")]
    public class CommunityController : Controller
    {
        private readonly CommunityMemberApiService _communityApiService;

        public CommunityController(CommunityMemberApiService communityApiService)
        {
            _communityApiService = communityApiService;
        }

        /// <summary>
        /// List all community members.
        /// URL: /Community/Index?apartmentId={guid}
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Index(Guid? apartmentId = null)
        {
            // ⭐ Pass apartmentId to the API so it can filter
            var response = await _communityApiService.GetAllCommunityMembersAsync(apartmentId);

            var viewModel = response?.Success == true && response.Data != null
                ? CommunityMemberViewModelMapper.From(response.Data)
                : new List<CommunityMemberViewModel>();

            ViewBag.ApartmentId = apartmentId; // keep it in scope for links
            return View(viewModel);
        }

        /// <summary>
        /// Show the "Assign Community Role" form.
        /// URL: /Community/AssignRole?apartmentId={guid}&role=President
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> AssignRole(Guid? apartmentId = null, string? role = null)
        {
            if (!apartmentId.HasValue)
            {
                TempData["ErrorMessage"] = "Apartment ID is required to assign a community role.";
                return Redirect("/");
            }

            // ⭐ Load eligible residents ONLY for this apartment
            var eligibleResponse = await _communityApiService.GetEligibleResidentsAsync(apartmentId.Value);
            var eligibleResidents = eligibleResponse?.Success == true && eligibleResponse.Data != null
                ? EligibleResidentViewModelMapper.From(eligibleResponse.Data)
                : new List<EligibleResidentViewModel>();

            ViewBag.EligibleResidents = eligibleResidents;

            var model = new AssignCommunityRoleViewModel
            {
                ApartmentId = apartmentId.Value,
                CommunityRole = role ?? string.Empty  // Pre-select the role if passed from Details page
            };

            return View(model);
        }

        /// <summary>
        /// POST – actually assign the role
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AssignRole(AssignCommunityRoleViewModel model)
        {
            if (model.UserId == Guid.Empty)
            {
                ModelState.AddModelError(nameof(model.UserId), "Please select a resident.");
            }
            if (string.IsNullOrEmpty(model.CommunityRole))
            {
                ModelState.AddModelError(nameof(model.CommunityRole), "Please select a role.");
            }
            if (!model.ApartmentId.HasValue)
            {
                ModelState.AddModelError(nameof(model.ApartmentId), "Apartment is required.");
            }

            if (!ModelState.IsValid)
            {
                // Reload eligible residents on validation failure
                var eligibleResponse = await _communityApiService.GetEligibleResidentsAsync(model.ApartmentId ?? Guid.Empty);
                ViewBag.EligibleResidents = eligibleResponse?.Success == true && eligibleResponse.Data != null
                    ? EligibleResidentViewModelMapper.From(eligibleResponse.Data)
                    : new List<EligibleResidentViewModel>();

                return View(model);
            }

            var request = new AssignCommunityRoleRequest
            {
                UserId = model.UserId,
                CommunityRole = model.CommunityRole,
                ApartmentId = model.ApartmentId!.Value  // ⭐ send apartmentId
            };

            var response = await _communityApiService.AssignCommunityRoleAsync(request);

            if (response?.Success == true)
            {
                TempData["SuccessMessage"] = $"{model.CommunityRole} role assigned successfully!";
                return RedirectToAction(nameof(Index), new { apartmentId = model.ApartmentId });
            }

            TempData["ErrorMessage"] = response?.Message ?? "Failed to assign role.";
            return RedirectToAction(nameof(Index), new { apartmentId = model.ApartmentId });
        }

        /// <summary>
        /// POST – remove a community role
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveRole(Guid userId, Guid? apartmentId = null)
        {
            var request = new RemoveCommunityRoleRequest { UserId = userId };

            var response = await _communityApiService.RemoveCommunityRoleAsync(request);

            if (response?.Success == true)
                TempData["SuccessMessage"] = "Community role removed successfully.";
            else
                TempData["ErrorMessage"] = response?.Message ?? "Failed to remove role.";

            return RedirectToAction(nameof(Index), new { apartmentId = apartmentId });
        }
    }
}
















/*
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ApartmentManagementSystem.Web.Services;
using ApartmentManagementSystem.Web.ViewModels.Community;
//Delete this..
namespace ApartmentManagementSystem.Web.Controllers
{
    [Authorize(Roles = "SuperAdmin,Manager")]
    public class CommunityController : Controller
    {
        private readonly CommunityMemberApiService _communityService;

        public CommunityController(CommunityMemberApiService communityService)
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


*/