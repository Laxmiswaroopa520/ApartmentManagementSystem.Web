using ApartmentManagementSystem.Web.Constants;
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










/*
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
        private readonly CommunityMemberApiService CommunityApiService;

        public CommunityController(CommunityMemberApiService communityApiService)
        {
            CommunityApiService = communityApiService;
        }

        // List all community members.
        // URL: /Community/Index?apartmentId={guid}
        [HttpGet]
        public async Task<IActionResult> Index(Guid? apartmentId = null)
        {
            //  Pass apartmentId to the API so it can filter
            var response = await CommunityApiService.GetAllCommunityMembersAsync(apartmentId);

            var viewModel = response?.Success == true && response.Data != null
                ? CommunityMemberViewModelMapper.From(response.Data)
                : new List<CommunityMemberViewModel>();

            ViewBag.ApartmentId = apartmentId; // keep it in scope for links
            return View(viewModel);
        }

        // Show the "Assign Community Role" form.
        // URL: /Community/AssignRole?apartmentId={guid}&role=President
        [HttpGet]
        public async Task<IActionResult> AssignRole(Guid? apartmentId = null, string? role = null)
        {
            if (!apartmentId.HasValue)
            {
                TempData["ErrorMessage"] = "Apartment ID is required to assign a community role.";
                return Redirect("/");
            }

            //  Load eligible residents ONLY for this apartment
            var eligibleResponse = await CommunityApiService.GetEligibleResidentsAsync(apartmentId.Value);
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

        // POST – actually assign the role
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
                var eligibleResponse = await CommunityApiService.GetEligibleResidentsAsync(model.ApartmentId ?? Guid.Empty);
                ViewBag.EligibleResidents = eligibleResponse?.Success == true && eligibleResponse.Data != null
                    ? EligibleResidentViewModelMapper.From(eligibleResponse.Data)
                    : new List<EligibleResidentViewModel>();

                return View(model);
            }

            var request = new AssignCommunityRoleRequest
            {
                UserId = model.UserId,
                CommunityRole = model.CommunityRole,
                ApartmentId = model.ApartmentId!.Value  //  send apartmentId
            };

            var response = await CommunityApiService.AssignCommunityRoleAsync(request);

            if (response?.Success == true)
            {
                TempData["SuccessMessage"] = $"{model.CommunityRole} role assigned successfully!";
                return RedirectToAction(nameof(Index), new { apartmentId = model.ApartmentId });
            }

            TempData["ErrorMessage"] = response?.Message ?? "Failed to assign role.";
            return RedirectToAction(nameof(Index), new { apartmentId = model.ApartmentId });
        }

        //remove a community role
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveRole(Guid userId, Guid? apartmentId = null)
        {
            var request = new RemoveCommunityRoleRequest { UserId = userId };

            var response = await CommunityApiService.RemoveCommunityRoleAsync(request);

            if (response?.Success == true)
                TempData["SuccessMessage"] = "Community role removed successfully.";
            else
                TempData["ErrorMessage"] = response?.Message ?? "Failed to remove role.";

            return RedirectToAction(nameof(Index), new { apartmentId = apartmentId });
        }
    }
}
*/















