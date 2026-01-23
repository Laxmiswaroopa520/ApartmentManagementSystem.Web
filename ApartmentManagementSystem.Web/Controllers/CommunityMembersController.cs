using ApartmentManagementSystem.Web.Services;
using ApartmentManagementSystem.Web.Services.DTOs.Community;
using ApartmentManagementSystem.Web.ViewModels.Community;
using ApartmentManagementSystem.Web.ViewModels.Dashboard;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Authorize(Roles = "SuperAdmin,Manager")]
public class CommunityMembersController : Controller
{
    private readonly CommunityMemberApiService CommunityApiservice;

    public CommunityMembersController(CommunityMemberApiService communityApiService)
    {
        CommunityApiservice = communityApiService;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var response = await CommunityApiservice.GetAllCommunityMembersAsync();

        var viewModel = response?.Data?
            .Select(m => new CommunityMemberViewModel
            {
                UserId = m.UserId,
                FullName = m.FullName,
                Email = m.Email,
                Phone = m.Phone,
                FlatNumber = m.FlatNumber,
                Role = m.Role,
                AssignedOn = m.AssignedOn,
                IsActive = m.IsActive
            })
            .ToList() ?? new List<CommunityMemberViewModel>();

        return View(viewModel);
    }
    // Assign Role
    [HttpGet]
    public async Task<IActionResult> AssignRole()
    {
        var response = await CommunityApiservice.GetEligibleResidentsAsync();

        var eligibleList = response?.Data?
            .Select(r => new EligibleResidentViewModel
            {
                UserId = r.UserId,
                DisplayText = $"{r.FullName} - Flat {r.FlatNumber}"
            })
            .ToList() ?? new List<EligibleResidentViewModel>();

        ViewBag.EligibleResidents = eligibleList;

        return View(new AssignCommunityRoleViewModel());
    }

  
    // POST: Assign Role
    [HttpPost]
    public async Task<IActionResult> AssignRole(AssignCommunityRoleViewModel model)
    {
        if (!ModelState.IsValid)
        {
            var response = await CommunityApiservice.GetEligibleResidentsAsync();

            ViewBag.EligibleResidents = response?.Data?
                .Select(r => new EligibleResidentViewModel
                {
                    UserId = r.UserId,
                    DisplayText = $"{r.FullName} - Flat {r.FlatNumber}"
                })
                .ToList() ?? new List<EligibleResidentViewModel>();

            return View(model);
        }

        var request = new AssignCommunityRoleRequest
        {
            UserId = model.UserId,
            CommunityRole = model.CommunityRole
        };

        var result = await CommunityApiservice.AssignCommunityRoleAsync(request);

        if (result?.Success == true)
        {
            TempData["SuccessMessage"] = $"{model.CommunityRole} assigned successfully";
            return RedirectToAction(nameof(Index));
        }

        ModelState.AddModelError("", result?.Message ?? "Failed to assign role");

        // reload dropdown again on failure
        var retryResponse = await CommunityApiservice.GetEligibleResidentsAsync();
        ViewBag.EligibleResidents = retryResponse?.Data?
            .Select(r => new EligibleResidentViewModel
            {
                UserId = r.UserId,
                DisplayText = $"{r.FullName} - Flat {r.FlatNumber}"
            })
            .ToList() ?? new List<EligibleResidentViewModel>();

        return View(model);
    }

    // POST: Remove Role
     [HttpPost]
    public async Task<IActionResult> RemoveRole(Guid userId)
    {
        var request = new RemoveCommunityRoleRequest { UserId = userId };
        var result = await CommunityApiservice.RemoveCommunityRoleAsync(request);

        if (result?.Success == true)
        {
            TempData["SuccessMessage"] = "Community role removed successfully";
        }
        else
        {
            TempData["ErrorMessage"] = result?.Message ?? "Failed to remove role";
        }

        return RedirectToAction(nameof(Index));
    }
}






























/*using ApartmentManagementSystem.Web.Services;
using ApartmentManagementSystem.Web.ViewModels.Community;
using ApartmentManagementSystem.Web.Services.DTOs.Community;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ApartmentManagementSystem.Web.ViewModels.Dashboard;

namespace ApartmentManagementSystem.Web.Controllers;

[Authorize(Roles = "SuperAdmin,Manager")]
public class CommunityMembersController : Controller
{
    private readonly CommunityMemberApiService _communityApiService;

    public CommunityMembersController(CommunityMemberApiService communityApiService)
    {
        _communityApiService = communityApiService;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var response = await _communityApiService.GetAllCommunityMembersAsync();

        var viewModel = response?.Data?
            .Select(m => new CommunityMemberViewModel
            {
                UserId = m.UserId,
                FullName = m.FullName,
                Email = m.Email,
                Phone = m.Phone,
                FlatNumber = m.FlatNumber,
                Role = m.Role,
                AssignedOn = m.AssignedOn,
                IsActive = m.IsActive
            })
            .ToList() ?? new List<CommunityMemberViewModel>();

        return View(viewModel);
    }

    [HttpGet]
    public async Task<IActionResult> AssignRole()
    {
        var response = await _communityApiService.GetEligibleResidentsAsync();

        //FIXED: Remove ?? operator with proper type
        /* var eligibleList = response?.Data?
             .Select(r => new
             {
                 UserId = r.UserId,
                 DisplayText = $"{r.FullName} - Flat {r.FlatNumber}"
             })
             .ToList();

         //        ViewBag.EligibleResidents = eligibleList ?? new List<dynamic>();
       //  ViewBag.EligibleResidents = eligibleList ?? new List<object>();

 ------------>
var eligibleList = response?.Data?
     .Select(r => new EligibleResidentViewModel
     {
         UserId = r.UserId,
         DisplayText = $"{r.FullName} - Flat {r.FlatNumber}"
     })
     .ToList() ?? new List<EligibleResidentViewModel>();

        ViewBag.EligibleResidents = eligibleList;

        return View(new AssignCommunityRoleViewModel());
    }

    [HttpPost]
    public async Task<IActionResult> AssignRole(AssignCommunityRoleViewModel model)
    {
        if (!ModelState.IsValid)
        {
            var response = await _communityApiService.GetEligibleResidentsAsync();

            /*            var eligibleList = response?.Data?
                            .Select(r => new
                            {
                                UserId = r.UserId,
                                DisplayText = $"{r.FullName} - Flat {r.FlatNumber}"
                            })
                            .ToList();

                        ViewBag.EligibleResidents = eligibleList ?? new List<dynamic>();

                        return View(model);
                    }-------->

            var eligibleList = response?.Data?
    .Select(r => new EligibleResidentViewModel
    {
        UserId = r.UserId,
        DisplayText = $"{r.FullName} - Flat {r.FlatNumber}"
    })
    .ToList() ?? new List<EligibleResidentViewModel>();

            ViewBag.EligibleResidents = eligibleList;
           // return View(model);

            var request = new AssignCommunityRoleRequest
            {
                UserId = model.UserId,
                CommunityRole = model.CommunityRole
            };

            var result = await _communityApiService.AssignCommunityRoleAsync(request);

            if (result?.Success == true)
            {
                TempData["SuccessMessage"] = $"{model.CommunityRole} assigned successfully";
                return RedirectToAction(nameof(Index));
            }

            ModelState.AddModelError("", result?.Message ?? "Failed to assign role");
            return View(model);
        }
    }

        [HttpPost]
        public async Task<IActionResult> RemoveRole(Guid userId)
        {
            var request = new RemoveCommunityRoleRequest { UserId = userId };
            var result = await _communityApiService.RemoveCommunityRoleAsync(request);

            if (result?.Success == true)
            {
                TempData["SuccessMessage"] = "Community role removed successfully";
            }
            else
            {
                TempData["ErrorMessage"] = result?.Message ?? "Failed to remove role";
            }

            return RedirectToAction(nameof(Index));
        }
    }
}


*/


























/*using ApartmentManagementSystem.Web.Services;
using ApartmentManagementSystem.Web.ViewModels.Community;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ApartmentManagementSystem.Web.Services.DTOs.Community;
namespace ApartmentManagementSystem.Web.Controllers;

[Authorize(Roles = "SuperAdmin,Manager")]
public class CommunityMembersController : Controller
{
    private readonly CommunityMemberApiService _communityApiService;

    public CommunityMembersController(CommunityMemberApiService communityApiService)
    {
        _communityApiService = communityApiService;
    }

    // =========================
    // GET: List All Community Members
    // =========================
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var response = await _communityApiService.GetAllCommunityMembersAsync();

        var viewModel = response?.Data?
            .Select(m => new CommunityMemberViewModel
            {
                UserId = m.UserId,
                FullName = m.FullName,
                Email = m.Email,
                Phone = m.Phone,
                FlatNumber = m.FlatNumber,
                Role = m.Role,
                AssignedOn = m.AssignedOn,
                IsActive = m.IsActive
            })
            .ToList() ?? new List<CommunityMemberViewModel>();

        return View(viewModel);
    }

    // =========================
    // GET: Assign Community Role
    // =========================
    [HttpGet]
    public async Task<IActionResult> AssignRole()
    {
        var response = await _communityApiService.GetEligibleResidentsAsync();

        ViewBag.EligibleResidents = response?.Data?
            .Select(r => new
            {
                UserId = r.UserId,
                DisplayText = $"{r.FullName} - Flat {r.FlatNumber}"
            })
            .ToList() ?? new List<dynamic>();

        return View(new AssignCommunityRoleViewModel());
    }

    // =========================
    // POST: Assign Community Role
    // =========================
    [HttpPost]
    public async Task<IActionResult> AssignRole(AssignCommunityRoleViewModel model)
    {
        if (!ModelState.IsValid)
        {
            var response = await _communityApiService.GetEligibleResidentsAsync();
            ViewBag.EligibleResidents = response?.Data?
                .Select(r => new
                {
                    UserId = r.UserId,
                    DisplayText = $"{r.FullName} - Flat {r.FlatNumber}"
                })
                .ToList() ?? new List<dynamic>();

            return View(model);
        }

        var request = new AssignCommunityRoleRequest
        {
            UserId = model.UserId,
            CommunityRole = model.CommunityRole
        };

        var result = await _communityApiService.AssignCommunityRoleAsync(request);

        if (result?.Success == true)
        {
            TempData["SuccessMessage"] = $"{model.CommunityRole} assigned successfully";
            return RedirectToAction(nameof(Index));
        }

        ModelState.AddModelError("", result?.Message ?? "Failed to assign role");
        return View(model);
    }

    // =========================
    // POST: Remove Community Role
    // =========================
    [HttpPost]
    public async Task<IActionResult> RemoveRole(Guid userId)
    {
        var request = new RemoveCommunityRoleRequest { UserId = userId };
        var result = await _communityApiService.RemoveCommunityRoleAsync(request);

        if (result?.Success == true)
        {
            TempData["SuccessMessage"] = "Community role removed successfully";
        }
        else
        {
            TempData["ErrorMessage"] = result?.Message ?? "Failed to remove role";
        }

        return RedirectToAction(nameof(Index));
    }
}

*/