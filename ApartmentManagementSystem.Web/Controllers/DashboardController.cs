using ApartmentManagementSystem.Web.Mappers.Dashboard;
using ApartmentManagementSystem.Web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ApartmentManagementSystem.Web.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        private readonly EnhancedDashboardApiService _enhancedDashboardApi;
        private readonly DashboardApiService _basicDashboardApi;

        public DashboardController(
            EnhancedDashboardApiService enhancedDashboardApi,
            DashboardApiService basicDashboardApi)
        {
            _enhancedDashboardApi = enhancedDashboardApi;
            _basicDashboardApi = basicDashboardApi;
        }

        public async Task<IActionResult> Index()
        {
            var roles = User.FindAll(ClaimTypes.Role)
                            .Select(r => r.Value)
                            .ToList();

            // ⭐ UNIFIED ADMIN DASHBOARD (SuperAdmin + Manager + Community Leaders)
            if (IsAdminRole(roles))
            {
                var response = await _enhancedDashboardApi.GetEnhancedAdminDashboardAsync();
                if (response?.Success == true && response.Data != null)
                {
                    var vm = AdminDashboardViewModelMapper.From(response.Data);
                    return View("Index", vm); // Same dashboard for all admins
                }
            }

            // STAFF
            if (IsStaffRole(roles))
            {
                var response = await _enhancedDashboardApi.GetStaffDashboardAsync();
                if (response?.Success == true && response.Data != null)
                {
                    var vm = StaffDashboardViewModelMapper.From(response.Data);
                    return View("StaffDashboard", vm);
                }
            }

            // OWNER
            if (roles.Contains("ResidentOwner"))
            {
                var response = await _basicDashboardApi.GetOwnerDashboardAsync();
                if (response?.Success == true && response.Data != null)
                {
                    var vm = OwnerDashboardViewModelMapper.From(response.Data);
                    return View("OwnerDashboard", vm);
                }
            }

            // TENANT
            if (roles.Contains("Tenant"))
            {
                var response = await _basicDashboardApi.GetTenantDashboardAsync();
                if (response?.Success == true && response.Data != null)
                {
                    var vm = TenantDashboardViewModelMapper.From(response.Data);
                    return View("TenantDashboard", vm);
                }
            }

            return RedirectToAction("AccessDenied", "Home");
        }

        private static bool IsAdminRole(IEnumerable<string> roles) =>
            roles.Any(r => r is "SuperAdmin" or "Manager" or "President" or "Secretary" or "Treasurer");

        private static bool IsStaffRole(IEnumerable<string> roles) =>
            roles.Any(r => r is
                "Security" or "Plumber" or "Electrician" or
                "Carpenter" or "Sweeper" or "Gardener" or "MaintenanceStaff");
    }
}



































