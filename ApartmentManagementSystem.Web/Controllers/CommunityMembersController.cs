

using ApartmentManagementSystem.Web.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ApartmentManagementSystem.Web.Services;
using ApartmentManagementSystem.Web.ViewModels.Community;

namespace ApartmentManagementSystem.Web.Controllers
{
    /// <summary>
    /// Secondary community controller scoped to SuperAdmin and Manager.
    /// Primarily used for apartment-detail-level community role management.
    /// </summary>
    [Authorize(Roles = AppRoles.AdminAndManager)]
    public class CommunityMembersController : Controller
    {
        private readonly CommunityMemberApiService CommunityService;

        public CommunityMembersController(CommunityMemberApiService communityService)
        {
            CommunityService = communityService;
        }

        /// <summary>
        /// Displays all community members across all apartments.
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            try
            {
                var response = await CommunityService.GetAllCommunityMembersAsync();

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
                TempData[TempDataKeys.ErrorMessage] = AppMessages.CommunityLoadFailed;
                return View(new List<CommunityMemberViewModel>());
            }
        }

        /// <summary>
        /// Displays the Assign Role form for a specific apartment.
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> AssignRole(Guid? apartmentId, string? role)
        {
            if (!apartmentId.HasValue)
            {
                TempData[TempDataKeys.ErrorMessage] = AppMessages.ApartmentIdRequiredShort;
                return RedirectToAction(AppRoutes.Actions.Index);
            }

            try
            {
                var response = await CommunityService.GetEligibleResidentsAsync(apartmentId.Value);

                ViewBag.EligibleResidents = response?.Success == true && response.Data != null
                    ? response.Data
                    : new List<Services.DTOs.Community.ResidentListDto>();

                return View(new AssignCommunityRoleViewModel
                {
                    ApartmentId = apartmentId,
                    CommunityRole = role
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading eligible residents: {ex.Message}");
                TempData[TempDataKeys.ErrorMessage] = AppMessages.EligibleResidentsLoadFailed;
                return RedirectToAction(AppRoutes.Actions.Index);
            }
        }

        /// <summary>
        /// Processes community role assignment.
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AssignRole(AssignCommunityRoleViewModel model)
        {
            if (!ModelState.IsValid)
            {
                if (!model.ApartmentId.HasValue)
                {
                    TempData[TempDataKeys.ErrorMessage] = AppMessages.ApartmentIdRequiredShort;
                    return RedirectToAction(AppRoutes.Actions.Index);
                }

                var response = await CommunityService.GetEligibleResidentsAsync(model.ApartmentId.Value);
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

                var result = await CommunityService.AssignCommunityRoleAsync(request);

                if (result?.Success == true)
                {
                    TempData[TempDataKeys.SuccessMessage] =
                        string.Format(AppMessages.CommunityRoleAssignSuccess, model.CommunityRole);

                    return model.ApartmentId.HasValue
                        ? RedirectToAction(AppRoutes.Actions.Details,
                              AppRoutes.Controllers.ApartmentBuilder,
                              new { id = model.ApartmentId.Value })
                        : RedirectToAction(AppRoutes.Actions.Index);
                }

                TempData[TempDataKeys.ErrorMessage] = result?.Message ?? AppMessages.CommunityRoleAssignFailed;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error assigning role: {ex.Message}");
                TempData[TempDataKeys.ErrorMessage] = AppMessages.GenericError;
            }

            if (model.ApartmentId.HasValue)
            {
                var residentsResponse = await CommunityService
                    .GetEligibleResidentsAsync(model.ApartmentId.Value);
                ViewBag.EligibleResidents = residentsResponse?.Success == true && residentsResponse.Data != null
                    ? residentsResponse.Data
                    : new List<Services.DTOs.Community.ResidentListDto>();
            }

            return View(model);
        }

        /// <summary>
        /// Removes a community role from a resident.
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveRole(Guid userId)
        {
            try
            {
                var result = await CommunityService.RemoveCommunityRoleAsync(
                    new Services.DTOs.Community.RemoveCommunityRoleRequest { UserId = userId });

                TempData[result?.Success == true ? TempDataKeys.SuccessMessage : TempDataKeys.ErrorMessage] =
                    result?.Success == true
                        ? AppMessages.CommunityRoleRemoveSuccess
                        : result?.Message ?? AppMessages.CommunityRoleRemoveFailed;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error removing role: {ex.Message}");
                TempData[TempDataKeys.ErrorMessage] = AppMessages.GenericError;
            }

            return RedirectToAction(AppRoutes.Actions.Index);
        }
    }
}













/*using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ApartmentManagementSystem.Web.Services;
using ApartmentManagementSystem.Web.ViewModels.Community;

namespace ApartmentManagementSystem.Web.Controllers
{
    [Authorize(Roles = "SuperAdmin,Manager")]
    public class CommunityMembersController : Controller
    {
        private readonly CommunityMemberApiService CommunityService;

        public CommunityMembersController(CommunityMemberApiService communityService)
        {
            CommunityService = communityService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            try
            {
                var response = await CommunityService.GetAllCommunityMembersAsync();

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
                var response = await CommunityService.GetEligibleResidentsAsync(apartmentId.Value);

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

                var response = await CommunityService.GetEligibleResidentsAsync(model.ApartmentId.Value);
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

                var result = await CommunityService.AssignCommunityRoleAsync(request);

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
                var residentsResponse = await CommunityService.GetEligibleResidentsAsync(model.ApartmentId.Value);
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

                var result = await CommunityService.RemoveCommunityRoleAsync(request);

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
























