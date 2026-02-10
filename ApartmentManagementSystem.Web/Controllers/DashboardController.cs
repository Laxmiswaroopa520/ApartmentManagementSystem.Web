/*using ApartmentManagementSystem.Web.Mappers.Dashboard;
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

            // SuperAdmin - Enhanced Dashboard
            if (roles.Contains("SuperAdmin"))
            {
                var response = await _enhancedDashboardApi.GetEnhancedAdminDashboardAsync();
                if (response?.Success == true && response.Data != null)
                {
                    var vm = AdminDashboardViewModelMapper.From(response.Data);
                    return View("Index", vm);
                }
            }

            // Manager - Manager Dashboard
            if (roles.Contains("Manager"))
            {
                var response = await _enhancedDashboardApi.GetManagerDashboardAsync();
                if (response?.Success == true && response.Data != null)
                {
                    var vm = ManagerDashboardViewModelMapper.From(response.Data);
                    return View("ManagerDashboard", vm);
                }
            }

            // Community Leaders - Community Leader Dashboard
            if (roles.Any(r => r == "President" || r == "Secretary" || r == "Treasurer"))
            {
                var response = await _enhancedDashboardApi.GetCommunityLeaderDashboardAsync();
                if (response?.Success == true && response.Data != null)
                {
                    var vm = CommunityLeaderDashboardViewModelMapper.From(response.Data);
                    return View("CommunityLeaderDashboard", vm);
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

        private static bool IsStaffRole(IEnumerable<string> roles) =>
            roles.Any(r => r is
                "Security" or "Plumber" or "Electrician" or
                "Carpenter" or "Sweeper" or "Gardener" or "MaintenanceStaff");
    }
}
*/



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
            // ⭐ Get ALL roles for the user
            var roles = User.FindAll(ClaimTypes.Role)
                           .Select(r => r.Value)
                           .ToList();

            // ⭐ PRIORITY ORDER: Check highest privilege roles first

            // 1. SuperAdmin (highest priority)
            if (roles.Contains("SuperAdmin"))
            {
                return await LoadSuperAdminDashboard();
            }

            // 2. Manager
            if (roles.Contains("Manager"))
            {
                return await LoadManagerDashboard();
            }

            // 3. Community Leaders (President, Secretary, Treasurer)
            if (roles.Any(r => r == "President" || r == "Secretary" || r == "Treasurer"))
            {
                return await LoadCommunityLeaderDashboard();
            }

            // 4. ResidentOwner
            if (roles.Contains("ResidentOwner"))
            {
                return await LoadOwnerDashboard();
            }

            // 5. Tenant
            if (roles.Contains("Tenant"))
            {
                return await LoadTenantDashboard();
            }

            // 6. Staff (lowest priority)
            if (IsStaffRole(roles))
            {
                return await LoadStaffDashboard();
            }

            // No recognized role
            TempData["ErrorMessage"] = "You don't have permission to access the dashboard.";
            return RedirectToAction("AccessDenied", "Home");
        }

        private async Task<IActionResult> LoadSuperAdminDashboard()
        {
            try
            {
                var response = await _enhancedDashboardApi.GetEnhancedAdminDashboardAsync();

                if (response?.Success == true && response.Data != null)
                {
                    var vm = AdminDashboardViewModelMapper.From(response.Data);
                    return View("Index", vm);
                }

                TempData["ErrorMessage"] = response?.Message ?? "Failed to load dashboard";
                return View("Error");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Failed to load dashboard: {ex.Message}";
                return View("Error");
            }
        }

        private async Task<IActionResult> LoadManagerDashboard()
        {
            try
            {
                var response = await _enhancedDashboardApi.GetManagerDashboardAsync();

                if (response?.Success == true && response.Data != null)
                {
                    var vm = ManagerDashboardViewModelMapper.From(response.Data);
                    return View("ManagerDashboard", vm);
                }

                TempData["ErrorMessage"] = response?.Message ?? "Failed to load manager dashboard";
                return View("Error");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Failed to load manager dashboard: {ex.Message}";
                return View("Error");
            }
        }

        private async Task<IActionResult> LoadCommunityLeaderDashboard()
        {
            try
            {
                var response = await _enhancedDashboardApi.GetCommunityLeaderDashboardAsync();

                if (response?.Success == true && response.Data != null)
                {
                    var vm = CommunityLeaderDashboardViewModelMapper.From(response.Data);
                    return View("CommunityLeaderDashboard", vm);
                }

                TempData["ErrorMessage"] = response?.Message ?? "Failed to load community leader dashboard";
                return View("Error");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Failed to load community leader dashboard: {ex.Message}";
                return View("Error");
            }
        }

        private async Task<IActionResult> LoadOwnerDashboard()
        {
            try
            {
                var response = await _basicDashboardApi.GetOwnerDashboardAsync();

                if (response?.Success == true && response.Data != null)
                {
                    var vm = OwnerDashboardViewModelMapper.From(response.Data);
                    return View("OwnerDashboard", vm);
                }

                TempData["ErrorMessage"] = "Failed to load owner dashboard";
                return View("Error");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Failed to load owner dashboard: {ex.Message}";
                return View("Error");
            }
        }

        private async Task<IActionResult> LoadTenantDashboard()
        {
            try
            {
                var response = await _basicDashboardApi.GetTenantDashboardAsync();

                if (response?.Success == true && response.Data != null)
                {
                    var vm = TenantDashboardViewModelMapper.From(response.Data);
                    return View("TenantDashboard", vm);
                }

                TempData["ErrorMessage"] = "Failed to load tenant dashboard";
                return View("Error");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Failed to load tenant dashboard: {ex.Message}";
                return View("Error");
            }
        }

        private async Task<IActionResult> LoadStaffDashboard()
        {
            try
            {
                var response = await _enhancedDashboardApi.GetStaffDashboardAsync();

                if (response?.Success == true && response.Data != null)
                {
                    var vm = StaffDashboardViewModelMapper.From(response.Data);
                    return View("StaffDashboard", vm);
                }

                TempData["ErrorMessage"] = "Failed to load staff dashboard";
                return View("Error");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Failed to load staff dashboard: {ex.Message}";
                return View("Error");
            }
        }

        private static bool IsStaffRole(IEnumerable<string> roles) =>
            roles.Any(r => r is
                "Security" or "Plumber" or "Electrician" or
                "Carpenter" or "Sweeper" or "Gardener" or "MaintenanceStaff");
    }
}


/*Priority Order Explanation

SuperAdmin - Full system access
Manager - Building management
President/Secretary/Treasurer - Community leadership
ResidentOwner - Regular flat owner
Tenant - Renting a flat
Staff - Security, maintenance, etc.*/






/*using ApartmentManagementSystem.Web.Mappers.Dashboard;
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
*/



































