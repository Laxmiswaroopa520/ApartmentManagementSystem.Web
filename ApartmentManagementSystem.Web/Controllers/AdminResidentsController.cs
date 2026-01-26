using ApartmentManagementSystem.Web.Mappers.Admin;
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

        [HttpGet]
        public async Task<IActionResult> Pending()
        {
            var response = await _adminApiService.GetPendingResidentsAsync();
            var viewModel = response?.Success == true && response.Data != null
                ? PendingResidentViewModelMapper.From(response.Data)
                : new List<PendingResidentViewModel>();
            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> AssignFlat(Guid userId, string userName)
        {
            // ⭐ Get apartments for current user (filtered by role)
            var apartmentsResponse = await _adminApiService.GetApartmentsAsync();

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

        [HttpPost]
        public async Task<IActionResult> AssignFlat(AssignFlatViewModel model)
        {
            if (!model.ApartmentId.HasValue)
                ModelState.AddModelError(nameof(model.ApartmentId), "Please select an apartment.");

            if (!model.FloorId.HasValue)
                ModelState.AddModelError(nameof(model.FloorId), "Please select a floor.");

            if (!model.FlatId.HasValue)
                ModelState.AddModelError(nameof(model.FlatId), "Please select a flat.");

            if (!ModelState.IsValid)
            {
                await PopulateDropdownsAsync(model);
                return View(model);
            }

            var response = await _adminApiService.AssignFlatAsync(new AssignFlatRequest
            {
                UserId = model.UserId,
                FlatId = model.FlatId.Value
            });

            if (response?.Success == true)
            {
                TempData["SuccessMessage"] = response.Message ?? "Flat assigned successfully";
                return RedirectToAction(nameof(Pending));
            }

            ModelState.AddModelError("", response?.Message ?? "Failed to assign flat");
            await PopulateDropdownsAsync(model);
            return View(model);
        }

        // ⭐ NEW: Get floors by apartment (AJAX)
        [HttpGet]
        public async Task<JsonResult> GetFloorsByApartment(Guid apartmentId)
        {
            var response = await _adminApiService.GetFloorsByApartmentAsync(apartmentId);
            var floors = response?.Success == true && response.Data != null
                ? FloorDropdownMapper.From(response.Data)
                : new List<FloorDropdownViewModel>();
            return Json(floors);
        }

        [HttpGet]
        public async Task<JsonResult> GetFlatsByFloor(Guid floorId)
        {
            var response = await _adminApiService.GetVacantFlatsByFloorAsync(floorId);
            var flats = response?.Success == true && response.Data != null
                ? FlatOptionMapper.From(response.Data)
                : new List<FlatOption>();
            return Json(flats);
        }
        private async Task PopulateDropdownsAsync(AssignFlatViewModel model)
        {
            var apartmentsResponse = await _adminApiService.GetApartmentsAsync();
            model.Apartments = apartmentsResponse?.Success == true && apartmentsResponse.Data != null
                ? ApartmentDropdownMapper.From(apartmentsResponse.Data)
                : new List<ApartmentDropdownViewModel>();

            // If apartment is selected, load its floors
            if (model.ApartmentId.HasValue)
            {
                var floorsResponse = await _adminApiService.GetFloorsByApartmentAsync(model.ApartmentId.Value);
                model.Floors = floorsResponse?.Success == true && floorsResponse.Data != null
                    ? FloorDropdownMapper.From(floorsResponse.Data)
                    : new List<FloorDropdownViewModel>();
            }
        }
    }
}













/*
[Authorize(Roles = "SuperAdmin,Manager")]
public class AdminResidentsController : Controller
{
    private readonly AdminResidentApiService AdminApiService;

    public AdminResidentsController(AdminResidentApiService adminApiService)
    {
        AdminApiService = adminApiService;
    }

    [HttpGet]
    public async Task<IActionResult> Pending()
    {
        var response = await AdminApiService.GetPendingResidentsAsync();

        var viewModel = response?.Success == true && response.Data != null
            ? PendingResidentViewModelMapper.From(response.Data)
            : new List<PendingResidentViewModel>();

        return View(viewModel);
    }

    [HttpGet]
    public async Task<IActionResult> AssignFlat(Guid userId, string userName)
    {
        var floorsResponse = await AdminApiService.GetFloorsAsync();

        var model = new AssignFlatViewModel
        {
            UserId = userId,
            UserName = userName ?? string.Empty,
            Floors = floorsResponse?.Success == true && floorsResponse.Data != null
                ? FloorDropdownMapper.From(floorsResponse.Data)
                : new List<FloorDropdownViewModel>(),
            Flats = new List<FlatOption>()
        };

        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> AssignFlat(AssignFlatViewModel model)
    {
        if (!model.FloorId.HasValue)
            ModelState.AddModelError(nameof(model.FloorId), "Please select a floor.");

        if (!model.FlatId.HasValue)
            ModelState.AddModelError(nameof(model.FlatId), "Please select a flat.");

        if (!ModelState.IsValid)
        {
            await PopulateFloorsAsync(model);
            return View(model);
        }

        var response = await AdminApiService.AssignFlatAsync(new AssignFlatRequest
        {
            UserId = model.UserId,
            FlatId = model.FlatId.Value
        });

        if (response?.Success == true)
        {
            TempData["SuccessMessage"] = response.Message;
            return RedirectToAction(nameof(Pending));
        }

        ModelState.AddModelError("", response?.Message ?? "Failed to assign flat");
        await PopulateFloorsAsync(model);
        return View(model);
    }

    [HttpGet]
    public async Task<JsonResult> GetFlatsByFloor(Guid floorId)
    {
        var response = await AdminApiService.GetVacantFlatsByFloorAsync(floorId);

        var flats = response?.Success == true && response.Data != null
            ? FlatOptionMapper.From(response.Data)
            : new List<FlatOption>();

        return Json(flats);
    }

    private async Task PopulateFloorsAsync(AssignFlatViewModel model)
    {
        var floorsResponse = await AdminApiService.GetFloorsAsync();
        model.Floors = floorsResponse?.Success == true && floorsResponse.Data != null
            ? FloorDropdownMapper.From(floorsResponse.Data)
            : new List<FloorDropdownViewModel>();
    }
}


*/






























/*using ApartmentManagementSystem.Web.Services;
using ApartmentManagementSystem.Web.Services.DTOs.Admin;
using ApartmentManagementSystem.Web.ViewModels.Admin;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ApartmentManagementSystem.Web.Controllers
{
    [Authorize(Roles = "SuperAdmin,Manager")]
    public class AdminResidentsController : Controller
    {
        private readonly AdminResidentApiService AdminApiService;

        public AdminResidentsController(AdminResidentApiService adminApiService)
        {
            AdminApiService = adminApiService;
        }     
        // GET: Pending Residents
        [HttpGet]
        public async Task<IActionResult> Pending()
        {
            var response = await AdminApiService.GetPendingResidentsAsync();

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

        // GET: Assign Flat
        [HttpGet]
        public async Task<IActionResult> AssignFlat(Guid userId, string userName)
        {
            var floorsResponse = await AdminApiService.GetFloorsAsync();

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
        // POST: Assign Flat

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
                var floorsResponse = await AdminApiService.GetFloorsAsync();
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

            var response = await AdminApiService.AssignFlatAsync(request);

            if (response?.Success == true)
            {
                TempData["SuccessMessage"] = response.Message;
                return RedirectToAction(nameof(Pending));
            }

            ModelState.AddModelError("", response?.Message ?? "Failed to assign flat");
            return View(model);
        }     
        // AJAX: Get Vacant Flats By Floor   only for ui interaction
        [HttpGet]
        public async Task<JsonResult> GetFlatsByFloor(Guid floorId)
        {
            var response = await AdminApiService.GetVacantFlatsByFloorAsync(floorId);

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
*/