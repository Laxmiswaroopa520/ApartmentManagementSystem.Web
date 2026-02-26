// PLACE AT: Web/Controllers/ApartmentBuilderController.cs
// ACTION:   REPLACE your existing file completely.
//
// NOTE: Your existing controller (doc 1) is actually correct.
// The AssignManager and RemoveManager actions are wired up fine.
// The real fix is in ManagerWebDTOs.cs (file 1) — the DTO was
// missing fields so System.Text.Json silently dropped them.
// This file is provided complete so you have one clean copy.


using ApartmentManagementSystem.Web.Mappers.Apartment;
using ApartmentManagementSystem.Web.Services;
using ApartmentManagementSystem.Web.Services.DTOs;
using ApartmentManagementSystem.Web.Services.DTOs.Manager;
using ApartmentManagementSystem.Web.ViewModels.Apartment;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;

namespace ApartmentManagementSystem.Web.Controllers
{
    [Authorize(Roles = "SuperAdmin")]
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

        // CREATE PAGE
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        // MANAGE (list all apartments) 
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
                Console.WriteLine($"Error loading apartments: {ex.Message}");
                TempData["ErrorMessage"] = "Failed to load apartments";
                return View(new List<ApartmentListViewModel>());
            }
        }

        // ─── DETAILS ──────────────────────────────────────────────────────────
        [HttpGet]
        public async Task<IActionResult> Details(Guid id)
        {
            try
            {
                var response = await ApartmentApiService.GetApartmentDetailAsync(id);

                if (response?.Success == true && response.Data != null)
                {
                    var viewModel = ApartmentDetailMapper.From(response.Data);
                    return View(viewModel);
                }

                TempData["ErrorMessage"] = response?.Message ?? "Apartment not found";
                return RedirectToAction(nameof(ManageApartments));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading details: {ex.Message}");
                TempData["ErrorMessage"] = "Failed to load apartment details";
                return RedirectToAction(nameof(ManageApartments));
            }
        }

        //  VISUALIZE (3D/2D view) 
        // Calls GET /api/ApartmentManagement/{id}/diagram via ApartmentApiService.
        // The API returns ApartmentDiagramDto which MUST have Floors populated.
        // If Floors is empty, the bug is in GetApartmentDiagramAsync on the
        // Application service side — see FILE 4.
        [HttpGet]
        public async Task<IActionResult> Visualize(Guid id)
        {
            try
            {
                var response = await ApartmentApiService.GetApartmentDiagramAsync(id);

                if (response?.Success == true && response.Data != null)
                {
                    var viewModel = ApartmentDiagramMapper.From(response.Data);
                    return View(viewModel);
                }

                TempData["ErrorMessage"] = response?.Message ?? "Diagram not found";
                return RedirectToAction(nameof(Details), new { id });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading diagram: {ex.Message}");
                TempData["ErrorMessage"] = "Failed to load apartment diagram";
                return RedirectToAction(nameof(Details), new { id });
            }
        }

        // CREATE APARTMENT (JSON POST from apartment-builder.js) 
        [HttpPost]
        public async Task<IActionResult> CreateApartment([FromBody] CreateApartmentViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Json(new { success = false, message = "Invalid data provided" });
                }

                Console.WriteLine("=== CreateApartment Called ===");
                Console.WriteLine($"Model: Name={model.Name}, Floors={model.TotalFloors}, Flats={model.FlatsPerFloor}");

                var dto = CreateApartmentMapper.ToDto(model);
                var response = await ApartmentApiService.CreateApartmentAsync(dto);

                Console.WriteLine($"Response received: Success={response?.Success}");

                if (response?.Success == true)
                {
                    return Json(new
                    {
                        success = true,
                        message = response.Message ?? "Apartment created successfully!",
                        data = response.Data
                    });
                }

                return Json(new
                {
                    success = false,
                    message = response?.Message ?? "Failed to create apartment"
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"=== ERROR in CreateApartment === {ex.Message}");
                return Json(new { success = false, message = $"Error: {ex.Message}" });
            }
        }
        [HttpPost]
        public async Task<IActionResult> AssignManager([FromBody] AssignManagerRequest request)
        {
            try
            {
                Console.WriteLine($"=== AssignManager Called ===");
                Console.WriteLine($"ApartmentId={request.ApartmentId} | UserId={request.UserId} | IsExternal={request.IsExternalManager}");

                var response = await ManagerApiService.AssignManagerToApartmentAsync(request);

                if (response?.Success == true)
                {
                    return Json(new { success = true, message = "Manager assigned successfully!" });
                }

                return Json(new
                {
                    success = false,
                    message = response?.Message ?? "Failed to assign manager"
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error assigning manager: {ex.Message}");
                return Json(new { success = false, message = $"Error: {ex.Message}" });
            }
        }

        //  REMOVE MANAGER (JSON POST from apartment-details.js)
        // apartment-details.js POSTs: { apartmentId }
        // Forwards to POST /api/Manager/remove
        [HttpPost]
        public async Task<IActionResult> RemoveManager([FromBody] RemoveManagerRequest request)
        {
            try
            {
                Console.WriteLine($"=== RemoveManager Called === ApartmentId={request.ApartmentId}");

                var response = await ManagerApiService.RemoveManagerFromApartmentAsync(request);

                if (response?.Success == true)
                {
                    return Json(new { success = true, message = "Manager removed successfully!" });
                }

                return Json(new
                {
                    success = false,
                    message = response?.Message ?? "Failed to remove manager"
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error removing manager: {ex.Message}");
                return Json(new { success = false, message = $"Error: {ex.Message}" });
            }
        }
        [HttpGet]
        public async Task<IActionResult> GetApartmentResidents(Guid apartmentId)
        {
            try
            {
                var result = await ManagerApiService
                    .GetApartmentResidentsAsync(apartmentId);

                return Json(result);
            }
            catch (Exception ex)
            {
                return Json(ApiResponse<List<AvailableManagerDto>>
                    .ErrorResponse(ex.Message));
            }
        }


        //added this for getting resident owners from that specific apartment
        /*  [HttpGet]
          public async Task<IActionResult> GetApartmentResidents(Guid apartmentId)
          {
              var result = await _managerApiService.GetApartmentResidentsAsync(apartmentId);
              return Json(result);
          }*/

    }
}
















