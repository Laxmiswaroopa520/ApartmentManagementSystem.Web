using ApartmentManagementSystem.Web.Constants;
using ApartmentManagementSystem.Web.Mappers.Community;
using ApartmentManagementSystem.Web.Services;
using ApartmentManagementSystem.Web.Services.DTOs.Community;
using ApartmentManagementSystem.Web.ViewModels.Community;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ApartmentManagementSystem.Web.Controllers
{
    [Authorize(Roles = AppRoles.AdminManagerCommunity)]
    public class CommunityController : Controller
    {
        private readonly CommunityMemberApiService CommunityApiService;

        public CommunityController(CommunityMemberApiService communityApiService)
        {
            CommunityApiService = communityApiService;
        }

        [HttpGet]
        public async Task<IActionResult> Index(Guid? apartmentId = null)
        {
            if (!apartmentId.HasValue)
            {
                TempData[AppMessages.ErrorMessage] = AppMessages.ApartmentIdRequired;
                return Redirect("/Dashboard");
            }

            var response = await CommunityApiService.GetAllCommunityMembersAsync(apartmentId);

            var viewModel = response?.Success == true && response.Data != null
                ? CommunityMemberViewModelMapper.From(response.Data)
                : new List<CommunityMemberViewModel>();

            var apartmentsResponse = await CommunityApiService.GetAllApartmentsAsync();
            var apartmentName = apartmentsResponse?.Data?
                .FirstOrDefault(a => a.Id == apartmentId)?.Name ?? "Unknown Apartment";

            ViewBag.ApartmentId = apartmentId;
            ViewBag.ApartmentName = apartmentName;

            var assignedRoles = viewModel.Select(m => m.Role).Distinct().ToList();
            ViewBag.AssignedRoles = assignedRoles;
            ViewBag.AllRolesFilled = new[] { "President", "Secretary", "Treasurer" }
                .All(r => assignedRoles.Contains(r));

            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> AssignRole(Guid? apartmentId = null, string? role = null)
        {
            if (!apartmentId.HasValue)
            {
                TempData[AppMessages.ErrorMessage] = AppMessages.ApartmentIdRequired;
                return Redirect("/Dashboard");
            }

            var eligibleResponse = await CommunityApiService
                .GetEligibleResidentsAsync(apartmentId.Value);

            // Explicit type — no ambiguity
            var eligibleList = eligibleResponse?.Success == true && eligibleResponse.Data != null
                ? EligibleResidentViewModelMapper.From(eligibleResponse.Data)
                : new List<EligibleResidentViewModel>();

            ViewBag.EligibleResidents = eligibleList;

            var assignedRoles = await CommunityApiService
                .GetAssignedRolesForApartmentAsync(apartmentId.Value);
            var allRoles = new List<string> { "President", "Secretary", "Treasurer" };
            var availableRoles = allRoles.Where(r => !assignedRoles.Contains(r)).ToList();

            if (!availableRoles.Any())
            {
                TempData[AppMessages.ErrorMessage] =
                    "All community roles are already assigned for this apartment.";
                return RedirectToAction(nameof(Index), new { apartmentId });
            }

            var apartmentsResponse = await CommunityApiService.GetAllApartmentsAsync();
            var apartmentName = apartmentsResponse?.Data?
                .FirstOrDefault(a => a.Id == apartmentId)?.Name ?? string.Empty;

            return View(new AssignCommunityRoleViewModel
            {
                ApartmentId = apartmentId.Value,
                ApartmentName = apartmentName,
                CommunityRole = role != null && availableRoles.Contains(role) ? role : string.Empty,
                AvailableRoles = availableRoles
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AssignRole(AssignCommunityRoleViewModel model)
        {
            if (model.UserId == Guid.Empty)
                ModelState.AddModelError(nameof(model.UserId), AppMessages.SelectResident);
            if (string.IsNullOrEmpty(model.CommunityRole))
                ModelState.AddModelError(nameof(model.CommunityRole), AppMessages.SelectRole);
            if (!model.ApartmentId.HasValue)
                ModelState.AddModelError(nameof(model.ApartmentId), AppMessages.ApartmentRequired);

            if (!ModelState.IsValid)
            {
                var aptId = model.ApartmentId ?? Guid.Empty;
                var eligibleResponse = await CommunityApiService.GetEligibleResidentsAsync(aptId);

                List<EligibleResidentViewModel> eligibleList =
                    eligibleResponse?.Success == true && eligibleResponse.Data != null
                        ? EligibleResidentViewModelMapper.From(eligibleResponse.Data)
                        : new List<EligibleResidentViewModel>();

                ViewBag.EligibleResidents = eligibleList;

                var assignedRoles = await CommunityApiService
                    .GetAssignedRolesForApartmentAsync(aptId);
                model.AvailableRoles = new List<string> { "President", "Secretary", "Treasurer" }
                    .Where(r => !assignedRoles.Contains(r)).ToList();

                return View(model);
            }

            var response = await CommunityApiService.AssignCommunityRoleAsync(
                new AssignCommunityRoleRequest
                {
                    UserId = model.UserId,
                    CommunityRole = model.CommunityRole,
                    ApartmentId = model.ApartmentId!.Value
                });

            if (response?.Success == true)
            {
                TempData[AppMessages.SuccessMessage] =
                    $"{model.CommunityRole} role assigned successfully.";
                return RedirectToAction(nameof(Index), new { apartmentId = model.ApartmentId });
            }

            TempData[AppMessages.ErrorMessage] =
                response?.Message ?? AppMessages.CommunityRoleAssignFailed;
            return RedirectToAction(nameof(Index), new { apartmentId = model.ApartmentId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveRole(Guid userId, Guid? apartmentId = null)
        {
            var response = await CommunityApiService.RemoveCommunityRoleAsync(
                new RemoveCommunityRoleRequest { UserId = userId });

            TempData[response?.Success == true
                ? AppMessages.SuccessMessage
                : AppMessages.ErrorMessage] =
                    response?.Success == true
                        ? AppMessages.CommunityRoleRemoveSuccess
                        : response?.Message ?? AppMessages.CommunityRoleRemoveFailed;

            return RedirectToAction(nameof(Index), new { apartmentId });
        }
    }
}
















/*using ApartmentManagementSystem.Web.Constants;
using ApartmentManagementSystem.Web.Mappers.Community;
using ApartmentManagementSystem.Web.Services;
using ApartmentManagementSystem.Web.Services.DTOs.Community;
using ApartmentManagementSystem.Web.ViewModels.Community;
using ApartmentManagementSystem.Web.ViewModels.Dashboard;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ApartmentManagementSystem.Web.Controllers
{
    /// <summary>
    /// Manages community roles scoped to a specific apartment.
    /// Accessible by SuperAdmin, Manager, President, Secretary, and Treasurer.
    /// </summary>
    [Authorize(Roles = AppRoles.AdminManagerCommunity)]
    public class CommunityController : Controller
    {
        private readonly CommunityMemberApiService CommunityApiService;

        public CommunityController(CommunityMemberApiService communityApiService)
        {
            CommunityApiService = communityApiService;
        }

        /// <summary>
        /// Displays all community members for a given apartment.
        /// URL: /Community/Index?apartmentId={guid}
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Index(Guid? apartmentId = null)
        {
            var response = await CommunityApiService.GetAllCommunityMembersAsync(apartmentId);

            var viewModel = response?.Success == true && response.Data != null
                ? CommunityMemberViewModelMapper.From(response.Data)
                : new List<CommunityMemberViewModel>();

            ViewBag.ApartmentId = apartmentId;
            return View(viewModel);
        }

        /// <summary>
        /// Displays the Assign Community Role form.
        /// URL: /Community/AssignRole?apartmentId={guid}&role=President
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> AssignRole(Guid? apartmentId = null, string? role = null)
        {
            if (!apartmentId.HasValue)
            {
                TempData[AppMessages.ErrorMessage] = AppMessages.ApartmentIdRequired;
                return Redirect("/");
            }

            var eligibleResponse = await CommunityApiService.GetEligibleResidentsAsync(apartmentId.Value);

            ViewBag.EligibleResidents = eligibleResponse?.Success == true && eligibleResponse.Data != null
                ? EligibleResidentViewModelMapper.From(eligibleResponse.Data)
                : new List<EligibleResidentViewModel>();

            return View(new AssignCommunityRoleViewModel
            {
                ApartmentId = apartmentId.Value,
                CommunityRole = role ?? string.Empty
            });
        }

        /// <summary>
        /// Processes community role assignment for a resident.
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AssignRole(AssignCommunityRoleViewModel model)
        {
            if (model.UserId == Guid.Empty)
                ModelState.AddModelError(nameof(model.UserId), AppMessages.SelectResident);

            if (string.IsNullOrEmpty(model.CommunityRole))
                ModelState.AddModelError(nameof(model.CommunityRole), AppMessages.SelectRole);

            if (!model.ApartmentId.HasValue)
                ModelState.AddModelError(nameof(model.ApartmentId), AppMessages.ApartmentRequired);

            if (!ModelState.IsValid)
            {
                var eligibleResponse = await CommunityApiService
                    .GetEligibleResidentsAsync(model.ApartmentId ?? Guid.Empty);

                ViewBag.EligibleResidents = eligibleResponse?.Success == true && eligibleResponse.Data != null
                    ? EligibleResidentViewModelMapper.From(eligibleResponse.Data)
                    : new List<EligibleResidentViewModel>();

                return View(model);
            }

            var response = await CommunityApiService.AssignCommunityRoleAsync(new AssignCommunityRoleRequest
            {
                UserId = model.UserId,
                CommunityRole = model.CommunityRole,
                ApartmentId = model.ApartmentId!.Value
            });

            if (response?.Success == true)
            {
                TempData[AppMessages.SuccessMessage] =
                    string.Format(AppMessages.CommunityRoleAssignSuccess, model.CommunityRole);
                return RedirectToAction(nameof(Index), new { apartmentId = model.ApartmentId });
            }

            TempData[AppMessages.ErrorMessage] = response?.Message ?? AppMessages.CommunityRoleAssignFailed;
            return RedirectToAction(nameof(Index), new { apartmentId = model.ApartmentId });
        }

        /// <summary>
        /// Removes a community role from a resident.
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveRole(Guid userId, Guid? apartmentId = null)
        {
            var response = await CommunityApiService.RemoveCommunityRoleAsync(
                new RemoveCommunityRoleRequest { UserId = userId });

            TempData[response?.Success == true ? AppMessages.SuccessMessage : AppMessages.ErrorMessage] =
                response?.Success == true
                    ? AppMessages.CommunityRoleRemoveSuccess
                    : response?.Message ?? AppMessages.CommunityRoleRemoveFailed;

            return RedirectToAction(nameof(Index), new { apartmentId });
        }
    }
}
*/








