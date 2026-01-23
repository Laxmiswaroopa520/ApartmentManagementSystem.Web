using ApartmentManagementSystem.Web.Mappers;
using ApartmentManagementSystem.Web.Services;
using ApartmentManagementSystem.Web.ViewModels.Dashboard;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Reflection;
using System.Security.Claims;

namespace ApartmentManagementSystem.Web.Controllers;

[Authorize]
public class DashboardController : Controller
{
    private readonly EnhancedDashboardApiService EnhancedDashboardApi;
    private readonly DashboardApiService BasicDashboardApi;

    public DashboardController(
        EnhancedDashboardApiService enhancedDashboardApi,
        DashboardApiService basicDashboardApi)
    {
        EnhancedDashboardApi = enhancedDashboardApi;
        BasicDashboardApi = basicDashboardApi;
    }

    public async Task<IActionResult> Index()
    {
        var roles = User.FindAll(ClaimTypes.Role).Select(r => r.Value).ToList();

        // ADMIN / COMMUNITY LEADERs
        if (roles.Any(r =>
            r == "SuperAdmin" ||
            r == "Manager" ||
            r == "President" ||
            r == "Secretary" ||
            r == "Treasurer"))
        {
            var response = await EnhancedDashboardApi.GetEnhancedAdminDashboardAsync();

            if (response?.Success == true && response.Data != null)
            {
                var vm = new EnhancedDashboardViewModel
                {
                    FullName = response.Data.FullName,
                    Role = response.Data.Role,
                    AllRoles = response.Data.AllRoles,

                    Stats = new AdvancedDashboardStatsViewModel
                    {
                        TotalResidents = response.Data.Stats.TotalResidents,
                        TotalFlats = response.Data.Stats.TotalFlats,
                        OccupiedFlats = response.Data.Stats.OccupiedFlats,
                        VacantFlats = response.Data.Stats.VacantFlats,
                        PendingRegistrations = response.Data.Stats.PendingRegistrations,
                        TotalStaffMembers = response.Data.Stats.TotalStaffMembers,
                        ActiveStaffMembers = response.Data.Stats.ActiveStaffMembers,
                        CommunityMembers = response.Data.Stats.CommunityMembers,
                        PendingComplaints = response.Data.Stats.PendingComplaints,
                        ResolvedComplaintsThisMonth = response.Data.Stats.ResolvedComplaintsThisMonth,
                        TotalOutstandingBills = response.Data.Stats.TotalOutstandingBills,
                        CollectionThisMonth = response.Data.Stats.CollectionThisMonth,
                        TodaysVisitors = response.Data.Stats.TodaysVisitors,
                        ActiveSecurityPersonnel = response.Data.Stats.ActiveSecurityPersonnel
                    },

                    RecentActivities = response.Data.RecentActivities
                        .Select(a => new RecentActivityViewModel
                        {
                            Activity = a.Activity,
                            Type = a.Type,
                            Timestamp = a.Timestamp
                        })
                        .ToList(),

                    QuickActions = response.Data.QuickActions
                        .Select(q => new QuickActionViewModel
                        {
                            Title = q.Title,
                            Icon = q.Icon,
                            Url = q.Url,
                            Color = q.Color,
                            RequiresPermission = q.RequiresPermission
                        })
                        .ToList(),

                    FinancialSummary = response.Data.FinancialSummary == null ? null :
                        new FinancialSummaryViewModel
                        {
                            TotalOutstanding = response.Data.FinancialSummary.TotalOutstanding,
                            CollectedThisMonth = response.Data.FinancialSummary.CollectedThisMonth,
                            CollectedLastMonth = response.Data.FinancialSummary.CollectedLastMonth,
                            PendingMaintenanceFees = response.Data.FinancialSummary.PendingMaintenanceFees,
                            PendingUtilityBills = response.Data.FinancialSummary.PendingUtilityBills,
                            Last6MonthsCollection = response.Data.FinancialSummary.Last6MonthsCollection
                                .Select(m => new MonthlyCollectionViewModel
                                {
                                    Month = m.Month,
                                    Amount = m.Amount
                                })
                                .ToList()
                        }
                };

                return View("Index", vm); 
            }
        }
        // STAFF DASHBOARD
        if (roles.Any(r =>
            r == "Security" ||
            r == "Plumber" ||
            r == "Electrician" ||
            r == "Carpenter" ||
            r == "Sweeper" ||
            r == "Gardener" ||
            r == "MaintenanceStaff"))
        {
            var response = await EnhancedDashboardApi.GetStaffDashboardAsync();

            if (response?.Success == true && response.Data != null)
            {
                var vm = new StaffDashboardViewModel
                {
                    FullName = response.Data.FullName,
                    StaffType = response.Data.StaffType,
                    ShiftStart = response.Data.ShiftStart,
                    ShiftEnd = response.Data.ShiftEnd,
                    TodaysTasks = response.Data.TodaysTasks,
                    CompletedTasks = response.Data.CompletedTasks,
                    PendingTasks = response.Data.PendingTasks,
                    MyTasks = response.Data.MyTasks
                        .Select(t => new TaskViewModel
                        {
                            TaskId = t.TaskId,
                            Title = t.Title,
                            Description = t.Description,
                            Priority = t.Priority,
                            DueDate = t.DueDate,
                            Status = t.Status
                        })
                        .ToList()
                };

                return View("StaffDashboard", vm);
            }
        }

        // =========================
        // OWNER DASHBOARD
        // =========================
        /*  if (roles.Contains("ResidentOwner"))
          {
              var response = await _basicDashboardApi.GetOwnerDashboardAsync();
              if (response?.Success == true)
                  return View("OwnerDashboard", response.Data);
          }*/
        if (roles.Contains("ResidentOwner"))
        {
            var response = await BasicDashboardApi.GetOwnerDashboardAsync();

            if (response?.Success == true)
            {
                var vm = OwnerDashboardViewModelMapper.From(response.Data);         //Instead of writing everything here to map from dto to view model i took mapper class..
                return View("OwnerDashboard", vm);
            }
        }


        // =========================
        // TENANT DASHBOARD
        // =========================
        /*  if (roles.Contains("Tenant"))
          {
              var response = await _basicDashboardApi.GetTenantDashboardAsync();
              if (response?.Success == true)
                  return View("TenantDashboard", response.Data);
          }

          return RedirectToAction("AccessDenied", "Home");*/
   
        // TENANT DASHBOARD (FIXED)
        if (roles.Contains("Tenant"))
           {
               var response = await BasicDashboardApi.GetTenantDashboardAsync();

               if (response?.Success == true && response.Data != null)
               {
                   var vm = new TenantDashboardViewModel
                   {
                       FullName = response.Data.FullName,
                       PendingComplaints = response.Data.PendingComplaints,
                       PendingRent = response.Data.PendingRent,

                       //  MyFlat = response.Data.MyFlat == null ? null : new TenantFlatViewModel
                       MyFlat = response.Data.MyFlat == null ? null : new FlatSummaryViewModel

                       {
                           FlatNumber = response.Data.MyFlat.FlatNumber,
                           ApartmentName = response.Data.MyFlat.ApartmentName,
                           OwnerName = response.Data.MyFlat.OwnerName
                       }
                   };

                   return View("TenantDashboard", vm);
               }
           }
        return RedirectToAction("AccessDenied", "Home");


    }
}






















/*using ApartmentManagementSystem.Web.Services;
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









*/



















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











