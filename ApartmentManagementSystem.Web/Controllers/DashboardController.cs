using ApartmentManagementSystem.Web.Services;
using ApartmentManagementSystem.Web.ViewModels.Dashboard;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
/*
namespace ApartmentManagementSystem.Web.Controllers;

[Authorize]
public class DashboardController : Controller
{
    public IActionResult Index()
    {
        var model = new DashboardViewModel
        {
            FullName = User.FindFirstValue(ClaimTypes.Name) ?? "",
            Role = User.FindFirstValue(ClaimTypes.Role) ?? "",
            Email = User.FindFirstValue(ClaimTypes.Email) ?? "",
            Phone = User.FindFirst("Phone")?.Value ?? "",
            FlatNumber = User.FindFirst("FlatNumber")?.Value
        };

        return View(model);
    }
}


*/

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
















/*[Authorize] // Protects entire controller
public class DashboardController : Controller
{
    private readonly DashboardApiService _dashboardApiService;

    public DashboardController(DashboardApiService dashboardApiService)
    {
        _dashboardApiService = dashboardApiService;
    }

    public async Task<IActionResult> Index()
    {
        // =========================
        // PRESERVED: Old working logic
        // =========================
        var userName = User.Identity?.Name ?? "User";
        var userRole = User.FindFirst(ClaimTypes.Role)?.Value ?? "Unknown";
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        ViewBag.UserName = userName;
        ViewBag.UserRole = userRole;
        ViewBag.UserId = userId;

        // =========================
        // NEW: Role-based dashboard routing
        // =========================
        return userRole switch
        {
            "SuperAdmin" or "President" or "Secretary" or "Treasurer"
                => await AdminDashboard(),

            "ResidentOwner"
                => await OwnerDashboard(),

            "Tenant"
                => await TenantDashboard(),

            _ => View("Error")
        };
    }

    // =========================
    // ADMIN DASHBOARD
    // =========================
    private async Task<IActionResult> AdminDashboard()
    {
        try
        {
            var response = await _dashboardApiService.GetAdminDashboardAsync();

            if (response?.Success == true && response.Data != null)
            {
                var viewModel = new AdminDashboardViewModel
                {
                    FullName = response.Data.FullName,
                    Role = response.Data.Role,
                    Stats = new DashboardStatsViewModel
                    {
                        TotalResidents = response.Data.Stats.TotalResidents,
                        TotalFlats = response.Data.Stats.TotalFlats,
                        OccupiedFlats = response.Data.Stats.OccupiedFlats,
                        VacantFlats = response.Data.Stats.VacantFlats,
                        PendingComplaints = response.Data.Stats.PendingComplaints,
                        PendingBills = response.Data.Stats.PendingBills,
                        TodaysVisitors = response.Data.Stats.TodaysVisitors
                    },
                    RecentActivities = response.Data.RecentActivities
                        .Select(a => new RecentActivityViewModel
                        {
                            Activity = a.Activity,
                            Timestamp = a.Timestamp,
                            Type = a.Type
                        })
                        .ToList()
                };

                return View("AdminDashboard", viewModel);
            }

            TempData["ErrorMessage"] = "Failed to load admin dashboard data.";
            return View("Error");
        }
        catch (Exception ex)
        {
            TempData["ErrorMessage"] = $"Error loading admin dashboard: {ex.Message}";
            return View("Error");
        }
    }

    // =========================
    // OWNER DASHBOARD
    // =========================
    private async Task<IActionResult> OwnerDashboard()
    {
        try
        {
            var response = await _dashboardApiService.GetOwnerDashboardAsync();

            if (response?.Success == true && response.Data != null)
            {
                var viewModel = new OwnerDashboardViewModel
                {
                    FullName = response.Data.FullName,
                    UserId = response.Data.UserId,
                    MyFlats = response.Data.MyFlats
                        .Select(f => new FlatSummaryViewModel
                        {
                            FlatId = f.FlatId,
                            FlatNumber = f.FlatNumber,
                            ApartmentName = f.ApartmentName,
                            OwnerName = f.OwnerName,
                            TenantName = f.TenantName
                        })
                        .ToList(),
                    PendingComplaints = response.Data.PendingComplaints,
                    PendingBills = response.Data.PendingBills,
                    TotalOutstanding = response.Data.TotalOutstanding
                };

                return View("OwnerDashboard", viewModel);
            }

            TempData["ErrorMessage"] = "Failed to load owner dashboard data.";
            return View("Error");
        }
        catch (Exception ex)
        {
            TempData["ErrorMessage"] = $"Error loading owner dashboard: {ex.Message}";
            return View("Error");
        }
    }

    // =========================
    // TENANT DASHBOARD
    // =========================
    private async Task<IActionResult> TenantDashboard()
    {
        try
        {
            var response = await _dashboardApiService.GetTenantDashboardAsync();

            if (response?.Success == true && response.Data != null)
            {
                FlatSummaryViewModel? flatViewModel = null;

                if (response.Data.MyFlat != null)
                {
                    flatViewModel = new FlatSummaryViewModel
                    {
                        FlatId = response.Data.MyFlat.FlatId,
                        FlatNumber = response.Data.MyFlat.FlatNumber,
                        ApartmentName = response.Data.MyFlat.ApartmentName,
                        OwnerName = response.Data.MyFlat.OwnerName,
                        TenantName = response.Data.MyFlat.TenantName
                    };
                }

                var viewModel = new TenantDashboardViewModel
                {
                    FullName = response.Data.FullName,
                    UserId = response.Data.UserId,
                    MyFlat = flatViewModel,
                    PendingComplaints = response.Data.PendingComplaints,
                    PendingRent = response.Data.PendingRent
                };

                return View("TenantDashboard", viewModel);
            }

            TempData["ErrorMessage"] = "Failed to load tenant dashboard data.";
            return View("Error");
        }
        catch (Exception ex)
        {
            TempData["ErrorMessage"] = $"Error loading tenant dashboard: {ex.Message}";
            return View("Error");
        }
    }
}

*/




















