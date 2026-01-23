using ApartmentManagementSystem.Web.Services;
using ApartmentManagementSystem.Web.Services.DTOs.Admin;
using ApartmentManagementSystem.Web.ViewModels.Admin;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ApartmentManagementSystem.Web.Controllers
{
    [Authorize(Roles = "SuperAdmin,Manager")]
    public class AdminResidentsController : Controller
    {
        private readonly AdminResidentApiService _adminApiService;

        public AdminResidentsController(AdminResidentApiService adminApiService)
        {
            _adminApiService = adminApiService;
        }

        // =========================
        // GET: Pending Residents
        // =========================
        [HttpGet]
        public async Task<IActionResult> Pending()
        {
            var response = await _adminApiService.GetPendingResidentsAsync();

            var viewModel = response?.Data?
                .Select(r => new PendingResidentViewModel
                {
                    UserId = r.UserId,
                    FullName = r.FullName,
                    PrimaryPhone = r.PrimaryPhone,
                    Email = r.Email,
                    ResidentType = r.ResidentType,
                    RegisteredOn = r.RegisteredOn,
                    Status = r.Status
                })
                .ToList() ?? new List<PendingResidentViewModel>();

            return View(viewModel);
        }

        // =========================
        // GET: Assign Flat
        // =========================
        [HttpGet]
        public async Task<IActionResult> AssignFlat(Guid userId, string userName)
        {
            var floorsResponse = await _adminApiService.GetFloorsAsync();

            var model = new AssignFlatViewModel
            {
                UserId = userId,
                UserName = userName ?? string.Empty,
                Floors = floorsResponse?.Data?
                    .Select(f => new FloorDropdownViewModel
                    {
                        Id = f.Id,
                        FloorNumber = f.FloorNumber
                    })
                    .ToList() ?? new List<FloorDropdownViewModel>(),

                Flats = new List<FlatOption>()   //  IMPORTANT
            };

            return View(model);
        }


       
        // =========================
        // POST: Assign Flat
        //

        [HttpPost]
        public async Task<IActionResult> AssignFlat(AssignFlatViewModel model)
        {
            if (!model.FloorId.HasValue)
            {
                ModelState.AddModelError("FloorId", "Please select a floor.");
            }

            if (!model.FlatId.HasValue)
            {
                ModelState.AddModelError("FlatId", "Please select a flat.");
            }

            if (!ModelState.IsValid)
            {
                var floorsResponse = await _adminApiService.GetFloorsAsync();
                model.Floors = floorsResponse?.Data?
                    .Select(f => new FloorDropdownViewModel
                    {
                        Id = f.Id,
                        FloorNumber = f.FloorNumber
                    })
                    .ToList() ?? new List<FloorDropdownViewModel>();

                return View(model);
            }

            var request = new AssignFlatRequest
            {
                UserId = model.UserId,
                FlatId = model.FlatId.Value // now safe
            };

            var response = await _adminApiService.AssignFlatAsync(request);

            if (response?.Success == true)
            {
                TempData["SuccessMessage"] = response.Message;
                return RedirectToAction(nameof(Pending));
            }

            ModelState.AddModelError("", response?.Message ?? "Failed to assign flat");
            return View(model);
        }

     
        // =========================
        // AJAX: Get Vacant Flats By Floor
        // =========================
        [HttpGet]
        public async Task<JsonResult> GetFlatsByFloor(Guid floorId)
        {
            var response = await _adminApiService.GetVacantFlatsByFloorAsync(floorId);

            if (response?.Success == true && response.Data != null)
            {
                // Convert API DTOs to FlatOption for Razor consumption
                var flats = response.Data.Select(f => new FlatOption
                {
                    Id = f.Id,
                    FlatNumber = f.FlatNumber
                }).ToList();

                return Json(flats);
            }

            return Json(new List<FlatOption>());
        }
    }
}
