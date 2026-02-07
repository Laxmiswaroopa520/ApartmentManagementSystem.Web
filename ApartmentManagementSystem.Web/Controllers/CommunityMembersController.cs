
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
            // Validate that apartmentId is provided
            if (!apartmentId.HasValue)
            {
                TempData["ErrorMessage"] = "Apartment ID is required";
                return RedirectToAction(nameof(Index));
            }

            try
            {
                var response = await _communityService.GetEligibleResidentsAsync(apartmentId.Value);

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
                // Validate apartmentId before calling the service
                if (!model.ApartmentId.HasValue)
                {
                    TempData["ErrorMessage"] = "Apartment ID is required";
                    return RedirectToAction(nameof(Index));
                }

                var response = await _communityService.GetEligibleResidentsAsync(model.ApartmentId.Value);
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

            // Make sure apartmentId exists before reloading residents
            if (model.ApartmentId.HasValue)
            {
                var residentsResponse = await _communityService.GetEligibleResidentsAsync(model.ApartmentId.Value);
                ViewBag.EligibleResidents = residentsResponse?.Success == true && residentsResponse.Data != null
                    ? residentsResponse.Data
                    : new List<Services.DTOs.Community.ResidentListDto>();
            }

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


























