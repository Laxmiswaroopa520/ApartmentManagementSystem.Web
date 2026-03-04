using ApartmentManagementSystem.Web.Constants;
using ApartmentManagementSystem.Web.Mappers.Admin;
using ApartmentManagementSystem.Web.Services;
using ApartmentManagementSystem.Web.Services.DTOs.Admin;
using ApartmentManagementSystem.Web.ViewModels.Admin;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ApartmentManagementSystem.Web.Controllers
{
    /// <summary>
    /// Handles resident administration: viewing pending residents
    /// and assigning flats. Restricted to SuperAdmin and Manager roles.
    /// </summary>
    [Authorize(Roles = AppRoles.AdminAndManager)]
    public class AdminResidentsController : Controller
    {
        private readonly AdminResidentApiService AdminApiService;

        public AdminResidentsController(AdminResidentApiService adminApiService)
        {
            AdminApiService = adminApiService;
        }

        /// <summary>
        /// Displays the list of residents awaiting flat assignment.
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Pending()
        {
            var response = await AdminApiService.GetPendingResidentsAsync();

            var viewModel = response?.Success == true && response.Data != null
                ? PendingResidentViewModelMapper.From(response.Data)
                : new List<PendingResidentViewModel>();

            return View(viewModel);
        }

        /// <summary>
        /// Displays the Assign Flat form for a specific resident.
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> AssignFlat(Guid userId, string userName)
        {
            var apartmentsResponse = await AdminApiService.GetApartmentsAsync();

            var model = new AssignFlatViewModel
            {
                UserId = userId,
                UserName = userName ?? string.Empty,
                Apartments = apartmentsResponse?.Success == true && apartmentsResponse.Data != null
                    ? ApartmentDropdownMapper.From(apartmentsResponse.Data)
                    : new List<ApartmentDropdownViewModel>(),
                Floors = new List<FloorDropdownViewModel>(),
                Flats = new List<FlatOption>()
            };

            return View(model);
        }

        /// <summary>
        /// Processes flat assignment. Redirects to Pending on success.
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> AssignFlat(AssignFlatViewModel model)
        {
            if (!model.ApartmentId.HasValue)
                ModelState.AddModelError(nameof(model.ApartmentId), AppMessages.SelectApartment);

            if (!model.FloorId.HasValue)
                ModelState.AddModelError(nameof(model.FloorId), AppMessages.SelectFloor);

            if (!model.FlatId.HasValue)
                ModelState.AddModelError(nameof(model.FlatId), AppMessages.SelectFlat);

            if (!ModelState.IsValid)
            {
                await PopulateDropdownsAsync(model);
                return View(model);
            }

            var response = await AdminApiService.AssignFlatAsync(new AssignFlatRequest
            {
                UserId = model.UserId,
                FlatId = model.FlatId!.Value
            });

            if (response?.Success == true)
            {
                TempData[AppMessages.SuccessMessage] = response.Message ?? AppMessages.FlatAssignSuccess;
                return RedirectToAction(nameof(Pending));
            }

            ModelState.AddModelError(string.Empty, response?.Message ?? AppMessages.FlatAssignFailed);
            await PopulateDropdownsAsync(model);
            return View(model);
        }

        /// <summary>
        /// Returns floors for a selected apartment (AJAX).
        /// </summary>
        [HttpGet]
        public async Task<JsonResult> GetFloorsByApartment(Guid apartmentId)
        {
            var response = await AdminApiService.GetFloorsByApartmentAsync(apartmentId);

            var floors = response?.Success == true && response.Data != null
                ? FloorDropdownMapper.From(response.Data)
                : new List<FloorDropdownViewModel>();

            return Json(floors);
        }

        /// <summary>
        /// Returns vacant flats for a selected floor (AJAX).
        /// </summary>
        [HttpGet]
        public async Task<JsonResult> GetFlatsByFloor(Guid floorId)
        {
            var response = await AdminApiService.GetVacantFlatsByFloorAsync(floorId);

            var flats = response?.Success == true && response.Data != null
                ? FlatOptionMapper.From(response.Data)
                : new List<FlatOption>();

            return Json(flats);
        }

        /// <summary>
        /// Re-populates dropdowns after a validation failure.
        /// </summary>
        private async Task PopulateDropdownsAsync(AssignFlatViewModel model)
        {
            var apartmentsResponse = await AdminApiService.GetApartmentsAsync();

            model.Apartments = apartmentsResponse?.Success == true && apartmentsResponse.Data != null
                ? ApartmentDropdownMapper.From(apartmentsResponse.Data)
                : new List<ApartmentDropdownViewModel>();

            if (model.ApartmentId.HasValue)
            {
                var floorsResponse = await AdminApiService
                    .GetFloorsByApartmentAsync(model.ApartmentId.Value);

                model.Floors = floorsResponse?.Success == true && floorsResponse.Data != null
                    ? FloorDropdownMapper.From(floorsResponse.Data)
                    : new List<FloorDropdownViewModel>();
            }
        }
    }
}


























