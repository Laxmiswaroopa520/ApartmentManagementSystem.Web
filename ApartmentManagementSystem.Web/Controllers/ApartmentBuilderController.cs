using ApartmentManagementSystem.Web.Constants;
using ApartmentManagementSystem.Web.Mappers.Apartment;
using ApartmentManagementSystem.Web.Services;
using ApartmentManagementSystem.Web.Services.DTOs;
using ApartmentManagementSystem.Web.Services.DTOs.Apartment;
using ApartmentManagementSystem.Web.Services.DTOs.Manager;
using ApartmentManagementSystem.Web.ViewModels.Apartment;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ApartmentManagementSystem.Web.Controllers
{
    /// <summary>
    /// Handles apartment CRUD, manager assignment/removal, and diagram visualisation.
    /// Restricted to SuperAdmin only.
    /// </summary>
    [Authorize(Roles = AppRoles.AdminOnly)]
    public class ApartmentBuilderController : Controller
    {
        private readonly ApartmentApiService ApartmentApiService;
        private readonly ManagerApiService ManagerApiService;

        /// <summary>
        /// Initialises the controller with apartment and manager API services.
        /// </summary>
        public ApartmentBuilderController(
            ApartmentApiService apartmentApiService,
            ManagerApiService managerApiService)
        {
            ApartmentApiService = apartmentApiService;
            ManagerApiService = managerApiService;
        }

        /// <summary>
        /// Displays the apartment creation wizard page.
        /// </summary>
        [HttpGet]
        public IActionResult Create() => View();

        /// <summary>
        /// Lists all apartments belonging to the SuperAdmin.
        /// On failure, shows an empty list with a TempData error message.
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> ManageApartments()
        {
            try
            {
                var response = await ApartmentApiService.GetAllApartmentsAsync();

                var viewModel = response?.Success == true && response.Data != null
                    ? ApartmentListMapper.From(response.Data)
                    : new List<ApartmentListViewModel>();

                return View(viewModel);
            }
            catch (Exception ex)
            {
                Console.WriteLine(AppMessages.ApartmentNotLoading + ex.Message);
                TempData[AppMessages.ErrorMessage] = AppMessages.ApartmentLoadFailed;
                return View(new List<ApartmentListViewModel>());
            }
        }

        /// <summary>
        /// Displays full details for a single apartment including manager
        /// and community leader assignments.
        /// Redirects to ManageApartments on failure.
        /// </summary>
        /// <param name="id">Unique identifier of the apartment.</param>
        [HttpGet]
        public async Task<IActionResult> Details(Guid id)
        {
            try
            {
                var response = await ApartmentApiService.GetApartmentDetailAsync(id);

                if (response?.Success == true && response.Data != null)
                    return View(ApartmentDetailMapper.From(response.Data));

                TempData[AppMessages.ErrorMessage] = response?.Message ?? AppMessages.ApartmentNotFound;
                return RedirectToAction(nameof(ManageApartments));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{ErrorMessages.ErrorLoadingDetails}: {ex.Message}");
                TempData[AppMessages.ErrorMessage] = AppMessages.ApartmentDetailFailed;
                return RedirectToAction(nameof(ManageApartments));
            }
        }

        /// <summary>
        /// Displays the floor-by-flat visual diagram for an apartment.
        /// Redirects to Details on failure.
        /// </summary>
        /// <param name="id">Unique identifier of the apartment.</param>
        [HttpGet]
        public async Task<IActionResult> Visualize(Guid id)
        {
            try
            {
                var response = await ApartmentApiService.GetApartmentDiagramAsync(id);

                if (response?.Success == true && response.Data != null)
                    return View(ApartmentDiagramMapper.From(response.Data));

                TempData[AppMessages.ErrorMessage] = response?.Message ?? AppMessages.ApartmentDiagramNotFound;
                return RedirectToAction(nameof(Details), new { id });
            }
            catch (Exception ex)
            {
                //  Console.WriteLine($"Error loading diagram: {ex.Message}");
                Console.WriteLine($"{ErrorMessages.ErrorLoadingDiagram}: {ex.Message}");
                TempData[AppMessages.ErrorMessage] = AppMessages.ApartmentDiagramFailed;
                return RedirectToAction(nameof(Details), new { id });
            }
        }

        /// <summary>
        /// Creates a new apartment with auto-generated floors and flats.
        /// Called via JSON POST from apartment-builder.js.
        /// Returns a JSON response — not a page redirect.
        /// </summary>
        /// <param name="model">Apartment creation view model from the request body.</param>
        [HttpPost]
        public async Task<IActionResult> CreateApartment([FromBody] CreateApartmentViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                    return Json(new { success = false, message = AppMessages.InvalidApartmentData });

                Console.WriteLine($"{AppMessages.CreateApartment}={model.Name}, Floors={model.TotalFloors}, Flats={model.FlatsPerFloor}");

                var response = await ApartmentApiService.CreateApartmentAsync(
                    CreateApartmentMapper.ToDto(model));

                return response?.Success == true
                    ? Json(new { success = true, message = response.Message ?? AppMessages.ApartmentCreateSuccess, data = response.Data })
                    : Json(new { success = false, message = response?.Message ?? AppMessages.ApartmentCreateFailed });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{ErrorMessages.ErrorCreateApartment}:{ex.Message}");
                return Json(new { success = false, message = ex.Message });
            }
        }

        /// <summary>
        /// Assigns a manager to an apartment.
        /// Called via JSON POST from apartment-details.js.
        /// Supports both existing resident and external manager assignment.
        /// </summary>
        /// <param name="request">Assignment request containing apartment ID, user info, and external flag.</param>
        [HttpPost]
        public async Task<IActionResult> AssignManager([FromBody] AssignManagerRequest request)
        {
            try
            {
                Console.WriteLine($"{AppMessages.AssignManager}={request.ApartmentId}, UserId={request.UserId}, IsExternal={request.IsExternalManager}");

                var response = await ManagerApiService.AssignManagerToApartmentAsync(request);

                return response?.Success == true
                    ? Json(new { success = true, message = AppMessages.ManagerAssignSuccess })
                    : Json(new { success = false, message = response?.Message ?? AppMessages.ManagerAssignFailed });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{ErrorMessages.ErrorAssignManager}:{ex.Message}");
                return Json(new { success = false, message = ex.Message });
            }
        }

        /// <summary>
        /// Removes the currently active manager from an apartment.
        /// Called via JSON POST from apartment-details.js.
        /// </summary>
        /// <param name="request">Request containing the apartment ID.</param>
        [HttpPost]
        public async Task<IActionResult> RemoveManager([FromBody] RemoveManagerRequest request)
        {
            try
            {
                Console.WriteLine($"{AppMessages.RemoveManager}={request.ApartmentId}");

                var response = await ManagerApiService.RemoveManagerFromApartmentAsync(request);

                return response?.Success == true
                    ? Json(new { success = true, message = AppMessages.ManagerRemoveSuccess })
                    : Json(new { success = false, message = response?.Message ?? AppMessages.ManagerRemoveFailed });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{ErrorMessages.ErrorRemovingManager}: {ex.Message}");
                return Json(new { success = false, message = ex.Message });
            }
        }

        /// <summary>
        /// Hard-deletes an apartment and all its child records.
        /// Blocked by the API if any flat is currently occupied.
        /// Called via JSON POST.
        /// </summary>
        /// <param name="request">Request containing the apartment ID to delete.</param>
        [HttpPost]
        public async Task<IActionResult> DeleteApartment([FromBody] DeleteApartmentRequest request)
        {
            try
            {
                Console.WriteLine($"{AppMessages.DeleteApartment}={request.ApartmentId}");

                var response = await ApartmentApiService.DeleteApartmentAsync(request.ApartmentId);

                return response?.Success == true
                    ? Json(new { success = true, message = AppMessages.ApartmentDeleteSuccess })
                    : Json(new { success = false, message = response?.Message ?? AppMessages.ApartmentDeleteFailed });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{ErrorMessages.ErrorDeletingApartment}: {ex.Message}");
                return Json(new { success = false, message = ex.Message });
            }
        }

        /// <summary>
        /// Returns resident owners in the specified apartment who are
        /// eligible to be assigned as manager or community leader.
        /// Used to populate assignment dropdowns in the UI.
        /// </summary>
        /// <param name="apartmentId">Unique identifier of the apartment.</param>
        [HttpGet]
        public async Task<IActionResult> GetApartmentResidents(Guid apartmentId)
        {
            try
            {
                var result = await ManagerApiService.GetApartmentResidentsAsync(apartmentId);
                return Json(result);
            }
            catch (Exception ex)
            {
                return Json(ApiResponse<List<AvailableManagerDto>>.ErrorResponse(ex.Message));
            }
        }
    }
}



































/*using ApartmentManagementSystem.Web.Constants;
using ApartmentManagementSystem.Web.Mappers.Apartment;
using ApartmentManagementSystem.Web.Services;
using ApartmentManagementSystem.Web.Services.DTOs;
using ApartmentManagementSystem.Web.Services.DTOs.Apartment;
using ApartmentManagementSystem.Web.Services.DTOs.Manager;
using ApartmentManagementSystem.Web.ViewModels.Apartment;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ApartmentManagementSystem.Web.Controllers
{
    /// <summary>
    /// Handles apartment CRUD, manager assignment/removal, and diagram visualization.
    /// Restricted to SuperAdmin only.
    /// </summary>
    [Authorize(Roles = AppRoles.SuperAdmin)]
    public class ApartmentBuilderController : Controller
    {
        private readonly ApartmentApiService ApartmentApiService;
        private readonly ManagerApiService ManagerApiService;

        public ApartmentBuilderController(
            ApartmentApiService apartmentApiService,
            ManagerApiService managerApiService)
        {
            ApartmentApiService = apartmentApiService;
            ManagerApiService = managerApiService;
        }

        /// <summary>
        /// Displays the apartment creation wizard page.
        /// </summary>
        [HttpGet]
        public IActionResult Create() => View();

        /// <summary>
        /// Lists all apartments belonging to the SuperAdmin.
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> ManageApartments()
        {
            try
            {
                var response = await ApartmentApiService.GetAllApartmentsAsync();

                var viewModel = response?.Success == true && response.Data != null
                    ? ApartmentListMapper.From(response.Data)
                    : new List<ApartmentListViewModel>();

                return View(viewModel);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ErrorMessages.ApartmentNotLoading+ { ex.Message});
                TempData[AppMessages.ErrorMessage] = AppMessages.ApartmentLoadFailed;
                return View(new List<ApartmentListViewModel>());
            }
        }

        /// <summary>
        /// Displays full details for a single apartment.
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Details(Guid id)
        {
            try
            {
                var response = await ApartmentApiService.GetApartmentDetailAsync(id);

                if (response?.Success == true && response.Data != null)
                    return View(ApartmentDetailMapper.From(response.Data));

                TempData[AppMessages.ErrorMessage] = response?.Message ?? AppMessages.ApartmentNotFound;
                return RedirectToAction(nameof(ManageApartments));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading details: {ex.Message}");
                TempData[AppMessages.ErrorMessage] = AppMessages.ApartmentDetailFailed;
                return RedirectToAction(nameof(ManageApartments));
            }
        }

        /// <summary>
        /// Displays the 3D/2D diagram view for an apartment.
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Visualize(Guid id)
        {
            try
            {
                var response = await ApartmentApiService.GetApartmentDiagramAsync(id);

                if (response?.Success == true && response.Data != null)
                    return View(ApartmentDiagramMapper.From(response.Data));

                TempData[AppMessages.ErrorMessage] = response?.Message ?? AppMessages.ApartmentDiagramNotFound;
                return RedirectToAction(nameof(Details), new { id });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading diagram: {ex.Message}");
                TempData[AppMessages.ErrorMessage] = AppMessages.ApartmentDiagramFailed;
                return RedirectToAction(nameof(Details), new { id });
            }
        }

        /// <summary>
        /// Creates a new apartment. Called via JSON POST from apartment-builder.js.
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> CreateApartment([FromBody] CreateApartmentViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                    return Json(new { success = false, message = AppMessages.InvalidApartmentData });

                Console.WriteLine($"CreateApartment: Name={model.Name}, Floors={model.TotalFloors}, Flats={model.FlatsPerFloor}");

                var response = await ApartmentApiService.CreateApartmentAsync(CreateApartmentMapper.ToDto(model));

                return response?.Success == true
                    ? Json(new { success = true, message = response.Message ?? AppMessages.ApartmentCreateSuccess, data = response.Data })
                    : Json(new { success = false, message = response?.Message ?? AppMessages.ApartmentCreateFailed });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in CreateApartment: {ex.Message}");
                return Json(new { success = false, message = ex.Message });
            }
        }

        /// <summary>
        /// Assigns a manager to an apartment. Called via JSON POST from apartment-details.js.
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> AssignManager([FromBody] AssignManagerRequest request)
        {
            try
            {
                Console.WriteLine($"AssignManager: ApartmentId={request.ApartmentId}, UserId={request.UserId}, IsExternal={request.IsExternalManager}");

                var response = await ManagerApiService.AssignManagerToApartmentAsync(request);

                return response?.Success == true
                    ? Json(new { success = true, message = AppMessages.ManagerAssignSuccess })
                    : Json(new { success = false, message = response?.Message ?? AppMessages.ManagerAssignFailed });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error assigning manager: {ex.Message}");
                return Json(new { success = false, message = ex.Message });
            }
        }

        /// <summary>
        /// Removes the current manager from an apartment. Called via JSON POST.
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> RemoveManager([FromBody] RemoveManagerRequest request)
        {
            try
            {
                Console.WriteLine($"RemoveManager: ApartmentId={request.ApartmentId}");

                var response = await ManagerApiService.RemoveManagerFromApartmentAsync(request);

                return response?.Success == true
                    ? Json(new { success = true, message = AppMessages.ManagerRemoveSuccess })
                    : Json(new { success = false, message = response?.Message ?? AppMessages.ManagerRemoveFailed });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error removing manager: {ex.Message}");
                return Json(new { success = false, message = ex.Message });
            }
        }

        /// <summary>
        /// Deletes an apartment. Called via JSON POST.
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> DeleteApartment([FromBody] DeleteApartmentRequest request)
        {
            try
            {
                Console.WriteLine($"DeleteApartment: ApartmentId={request.ApartmentId}");

                var response = await ApartmentApiService.DeleteApartmentAsync(request.ApartmentId);

                return response?.Success == true
                    ? Json(new { success = true, message = AppMessages.ApartmentDeleteSuccess })
                    : Json(new { success = false, message = response?.Message ?? AppMessages.ApartmentDeleteFailed });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting apartment: {ex.Message}");
                return Json(new { success = false, message = ex.Message });
            }
        }

        /// <summary>
        /// Returns residents for a specific apartment (used for manager/community assignment UI).
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetApartmentResidents(Guid apartmentId)
        {
            try
            {
                var result = await ManagerApiService.GetApartmentResidentsAsync(apartmentId);
                return Json(result);
            }
            catch (Exception ex)
            {
                return Json(ApiResponse<List<AvailableManagerDto>>.ErrorResponse(ex.Message));
            }
        }
    }
}
*/
















