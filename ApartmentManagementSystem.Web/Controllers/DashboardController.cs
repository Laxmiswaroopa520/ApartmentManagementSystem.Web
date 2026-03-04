
using ApartmentManagementSystem.Web.Constants;
using ApartmentManagementSystem.Web.Mappers.Dashboard;
using ApartmentManagementSystem.Web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ApartmentManagementSystem.Web.Controllers
{
    /// <summary>
    /// Routes authenticated users to their role-specific dashboard view.
    /// Priority order: SuperAdmin > Manager > Community Leader > Owner > Tenant > Staff.
    /// </summary>
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
            var roles = User.FindAll(ClaimTypes.Role)
                            .Select(r => r.Value)
                            .ToList();

            if (roles.Contains(AppRoles.SuperAdmin)) return await LoadSuperAdminDashboard();
            if (roles.Contains(AppRoles.Manager)) return await LoadManagerDashboard();

            if (roles.Any(r => r == AppRoles.President ||
                               r == AppRoles.Secretary ||
                               r == AppRoles.Treasurer))
                return await LoadCommunityLeaderDashboard();

            if (roles.Contains(AppRoles.ResidentOwner)) return await LoadOwnerDashboard();
            if (roles.Contains(AppRoles.Tenant)) return await LoadTenantDashboard();
            if (IsStaffRole(roles)) return await LoadStaffDashboard();

            TempData[AppMessages.ErrorMessage] = AppMessages.NoDashboardPermission;
            return RedirectToAction("AccessDenied", "Home");
        }

        private async Task<IActionResult> LoadSuperAdminDashboard()
        {
            try
            {
                var response = await EnhancedDashboardApi.GetEnhancedAdminDashboardAsync();

                if (response?.Success == true && response.Data != null)
                    return View("Index", AdminDashboardViewModelMapper.From(response.Data));

                TempData[AppMessages.ErrorMessage] = response?.Message ?? AppMessages.DashboardLoadFailed;
                return View("Error");
            }
            catch (Exception ex)
            {
                TempData[AppMessages.ErrorMessage] = $"{AppMessages.DashboardLoadFailed}: {ex.Message}";
                return View("Error");
            }
        }

        private async Task<IActionResult> LoadManagerDashboard()
        {
            try
            {
                var response = await EnhancedDashboardApi.GetManagerDashboardAsync();

                if (response?.Success == true && response.Data != null)
                    return View("ManagerDashboard", ManagerDashboardViewModelMapper.From(response.Data));

                TempData[AppMessages.ErrorMessage] = response?.Message ?? AppMessages.ManagerDashFailed;
                return View("Error");
            }
            catch (Exception ex)
            {
                TempData[AppMessages.ErrorMessage] = $"{AppMessages.ManagerDashFailed}: {ex.Message}";
                return View("Error");
            }
        }

        private async Task<IActionResult> LoadCommunityLeaderDashboard()
        {
            try
            {
                var response = await EnhancedDashboardApi.GetCommunityLeaderDashboardAsync();

                if (response?.Success == true && response.Data != null)
                    return View("CommunityLeaderDashboard", CommunityLeaderDashboardViewModelMapper.From(response.Data));

                TempData[AppMessages.ErrorMessage] = response?.Message ?? AppMessages.CommunityDashFailed;
                return View("Error");
            }
            catch (Exception ex)
            {
                TempData[AppMessages.ErrorMessage] = $"{AppMessages.CommunityDashFailed}: {ex.Message}";
                return View("Error");
            }
        }

        private async Task<IActionResult> LoadOwnerDashboard()
        {
            try
            {
                var response = await BasicDashboardApi.GetOwnerDashboardAsync();

                if (response?.Success == true && response.Data != null)
                    return View("OwnerDashboard", OwnerDashboardViewModelMapper.From(response.Data));

                TempData[AppMessages.ErrorMessage] = AppMessages.OwnerDashFailed;
                return View("Error");
            }
            catch (Exception ex)
            {
                TempData[AppMessages.ErrorMessage] = $"{AppMessages.OwnerDashFailed}: {ex.Message}";
                return View("Error");
            }
        }

        private async Task<IActionResult> LoadTenantDashboard()
        {
            try
            {
                var response = await BasicDashboardApi.GetTenantDashboardAsync();

                if (response?.Success == true && response.Data != null)
                    return View("TenantDashboard", TenantDashboardViewModelMapper.From(response.Data));

                TempData[AppMessages.ErrorMessage] = AppMessages.TenantDashFailed;
                return View("Error");
            }
            catch (Exception ex)
            {
                TempData[AppMessages.ErrorMessage] = $"{AppMessages.TenantDashFailed}: {ex.Message}";
                return View("Error");
            }
        }

        private async Task<IActionResult> LoadStaffDashboard()
        {
            try
            {
                var response = await EnhancedDashboardApi.GetStaffDashboardAsync();

                if (response?.Success == true && response.Data != null)
                    return View("StaffDashboard", StaffDashboardViewModelMapper.From(response.Data));

                TempData[AppMessages.ErrorMessage] = AppMessages.StaffDashFailed;
                return View("Error");
            }
            catch (Exception ex)
            {
                TempData[AppMessages.ErrorMessage] = $"{AppMessages.StaffDashFailed}: {ex.Message}";
                return View("Error");
            }
        }

        private static bool IsStaffRole(IEnumerable<string> roles) =>
            roles.Any(r => r is
                AppRoles.Security or AppRoles.Plumber or AppRoles.Electrician or
                AppRoles.Carpenter or AppRoles.Sweeper or AppRoles.Gardener or
                AppRoles.MaintenanceStaff);
    }
}




















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
            //  Get ALL roles for the user
            var roles = User.FindAll(ClaimTypes.Role)
                           .Select(r => r.Value)
                           .ToList();

            // PRIORITY ORDER: Check highest privilege roles first

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
                var response = await EnhancedDashboardApi.GetEnhancedAdminDashboardAsync();

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
                var response = await EnhancedDashboardApi.GetManagerDashboardAsync();

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
                var response = await EnhancedDashboardApi.GetCommunityLeaderDashboardAsync();

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
                var response = await BasicDashboardApi.GetOwnerDashboardAsync();

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
                var response = await BasicDashboardApi.GetTenantDashboardAsync();

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
                var response = await EnhancedDashboardApi.GetStaffDashboardAsync();

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
*/

/*Priority Order Explanation

SuperAdmin - Full system access
Manager - Building management
President/Secretary/Treasurer - Community leadership
ResidentOwner - Regular flat owner
Tenant - Renting a flat
Staff - Security, maintenance, etc.*/






































