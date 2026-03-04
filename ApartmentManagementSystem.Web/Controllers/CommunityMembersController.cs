using ApartmentManagementSystem.Web.Constants;
using ApartmentManagementSystem.Web.Services;
using ApartmentManagementSystem.Web.ViewModels.Community;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ApartmentManagementSystem.Web.Controllers
{
    /// <summary>
    /// Secondary community controller used for apartment-detail-level
    /// role management. Restricted to SuperAdmin and Manager.
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
                TempData[AppMessages.ErrorMessage] = AppMessages.CommunityLoadFailed;
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
                TempData[AppMessages.ErrorMessage] = AppMessages.ApartmentIdRequiredShort;
                return RedirectToAction(nameof(Index));
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
                TempData[AppMessages.ErrorMessage] = AppMessages.EligibleResidentsLoadFailed;
                return RedirectToAction(nameof(Index));
            }
        }

        /// <summary>
        /// Processes community role assignment.
        /// Redirects to ApartmentBuilder/Details if apartmentId is present, else to Index.
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AssignRole(AssignCommunityRoleViewModel model)
        {
            if (!ModelState.IsValid)
            {
                if (!model.ApartmentId.HasValue)
                {
                    TempData[AppMessages.ErrorMessage] = AppMessages.ApartmentIdRequiredShort;
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
                var result = await CommunityService.AssignCommunityRoleAsync(
                    new Services.DTOs.Community.AssignCommunityRoleRequest
                    {
                        UserId = model.UserId,
                        CommunityRole = model.CommunityRole
                    });

                if (result?.Success == true)
                {
                    TempData[AppMessages.SuccessMessage] =
                        string.Format(AppMessages.CommunityRoleAssignSuccess, model.CommunityRole);

                    return model.ApartmentId.HasValue
                        ? RedirectToAction("Details", "ApartmentBuilder", new { id = model.ApartmentId.Value })
                        : RedirectToAction(nameof(Index));
                }

                TempData[AppMessages.ErrorMessage] = result?.Message ?? AppMessages.CommunityRoleAssignFailed;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error assigning role: {ex.Message}");
                TempData[AppMessages.ErrorMessage] = AppMessages.GenericError;
            }

            if (model.ApartmentId.HasValue)
            {
                var residentsResponse = await CommunityService.GetEligibleResidentsAsync(model.ApartmentId.Value);
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

                TempData[result?.Success == true ? AppMessages.SuccessMessage : AppMessages.ErrorMessage] =
                    result?.Success == true
                        ? AppMessages.CommunityRoleRemoveSuccess
                        : result?.Message ?? AppMessages.CommunityRoleRemoveFailed;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error removing role: {ex.Message}");
                TempData[AppMessages.ErrorMessage] = AppMessages.GenericError;
            }

            return RedirectToAction(nameof(Index));
        }
    }
}













