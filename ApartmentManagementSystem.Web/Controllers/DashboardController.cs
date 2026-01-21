using ApartmentManagementSystem.Web.Services;
using ApartmentManagementSystem.Web.ViewModels.Dashboard;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ApartmentManagementSystem.Web.Controllers;

[Authorize]
public class DashboardController : Controller
{
    private readonly EnhancedDashboardApiService _dashboardApiService;

    public DashboardController(EnhancedDashboardApiService dashboardApiService)
    {
        _dashboardApiService = dashboardApiService;
    }

    public async Task<IActionResult> Index()
    {
        var roles = User.FindAll(ClaimTypes.Role).Select(r => r.Value).ToList();

      
        // ADMIN / COMMUNITY LEADERS
      
        if (roles.Any(r =>
            r == "SuperAdmin" ||
            r == "Manager" ||
            r == "President" ||
            r == "Secretary" ||
            r == "Treasurer"))
        {
            var response = await _dashboardApiService.GetEnhancedAdminDashboardAsync();

            if (response?.Success == true && response.Data != null)
            {
                var vm = new EnhancedDashboardViewModel
                {
                    FullName = response.Data.FullName,
                    AllRoles = response.Data.Roles,
                    Stats = response.Data.Stats,
                    RecentActivities = response.Data.RecentActivities
                };

                return View("Index", vm);
            }
        }

        // =========================
        // STAFF DASHBOARD
        // =========================
        if (roles.Any(r =>
            r == "Security" ||
            r == "Plumber" ||
            r == "Electrician" ||
            r == "Carpenter" ||
            r == "Sweeper" ||
            r == "Gardener" ||
            r == "MaintenanceStaff"))
        {
            var response = await _dashboardApiService.GetStaffDashboardAsync();

            if (response?.Success == true && response.Data != null)
            {
                var vm = new EnhancedDashboardViewModel
                {
                    FullName = User.Identity?.Name ?? "",
                    AllRoles = roles,
                    Stats = response.Data.Stats
                };

                return View("Index", vm);
            }
        }

    
        // FALLBACK
        // =========================
        return RedirectToAction("AccessDenied", "Auth");
    }
}





























/*
using ApartmentManagementSystem.Web.Services;
using ApartmentManagementSystem.Web.ViewModels.Dashboard;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ApartmentManagementSystem.Web.Controllers;

[Authorize]
public class DashboardController : Controller
{
    private readonly DashboardApiService _dashboardApiService;

    public DashboardController(DashboardApiService dashboardApiService)
    {
        _dashboardApiService = dashboardApiService;
    }

    public async Task<IActionResult> Index()
    {
        var role = User.FindFirstValue(ClaimTypes.Role) ?? "";

        // =========================
        // BASE MODEL (claims)
        // =========================
        var model = new DashboardViewModel
        {
            FullName = User.FindFirstValue(ClaimTypes.Name) ?? "",
            Role = role,
            Email = User.FindFirstValue(ClaimTypes.Email) ?? "",
            Phone = User.FindFirst("Phone")?.Value ?? "",
            FlatNumber = User.FindFirst("FlatNumber")?.Value ?? "Not Assigned"
        };

        // =========================
        // ADMIN DASHBOARD
        // =========================
        if (role == "SuperAdmin" || role == "President" ||
            role == "Secretary" || role == "Treasurer")
        {
            var response = await _dashboardApiService.GetAdminDashboardAsync();

            if (response?.Success == true && response.Data?.Stats != null)
            {
                model.TotalResidents = response.Data.Stats.TotalResidents;
                model.TotalFlats = response.Data.Stats.TotalFlats;
                model.OccupiedFlats = response.Data.Stats.OccupiedFlats;
                model.VacantFlats = response.Data.Stats.VacantFlats;

                // You are using this for "Pending Registrations" tile
                model.PendingRegistrations = response.Data.Stats.PendingComplaints;
            }
        }

        // =========================
        // OWNER DASHBOARD
        // =========================
        else if (role == "ResidentOwner")
        {
            var response = await _dashboardApiService.GetOwnerDashboardAsync();

            if (response?.Success == true && response.Data != null)
            {
                model.FlatNumber =
                    response.Data.MyFlats?.FirstOrDefault()?.FlatNumber
                    ?? "Not Assigned";
            }
        }

        // =========================
        // TENANT DASHBOARD
        // =========================
        else if (role == "Tenant")
        {
            var response = await _dashboardApiService.GetTenantDashboardAsync();

            if (response?.Success == true && response.Data?.MyFlat != null)
            {
                model.FlatNumber = response.Data.MyFlat.FlatNumber;
            }
        }

        return View(model);
    }
}


*/











