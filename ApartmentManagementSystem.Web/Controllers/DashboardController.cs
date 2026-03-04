
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
























