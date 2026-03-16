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
    ///
    /// Priority order for role resolution:
    /// SuperAdmin → Manager → Community Leader → ResidentOwner → Tenant → Staff
    ///
    /// If no matching role is found, redirects to AccessDenied.
    /// </summary>
    [Authorize]
    public class DashboardController : Controller
    {
        private readonly EnhancedDashboardApiService EnhancedDashboardApi;
        private readonly DashboardApiService BasicDashboardApi;

        /// <summary>
        /// Initialises the controller with enhanced and basic dashboard API services.
        /// </summary>
        public DashboardController(
            EnhancedDashboardApiService enhancedDashboardApi,
            DashboardApiService basicDashboardApi)
        {
            EnhancedDashboardApi = enhancedDashboardApi;
            BasicDashboardApi = basicDashboardApi;
        }

        /// <summary>
        /// Entry point for the dashboard.
        /// Reads role claims from the authenticated user and delegates
        /// to the appropriate role-specific load method.
        /// </summary>
        public async Task<IActionResult> Index()
        {
            var roles = User.FindAll(ClaimTypes.Role)
                            .Select(r => r.Value)
                            .ToList();

            if (roles.Contains(AppRoles.SuperAdmin))
                return await LoadSuperAdminDashboard();

            if (roles.Contains(AppRoles.Manager))
                return await LoadManagerDashboard();

            if (roles.Any(r => r == AppRoles.President ||
                               r == AppRoles.Secretary ||
                               r == AppRoles.Treasurer))
                return await LoadCommunityLeaderDashboard();

            if (roles.Contains(AppRoles.ResidentOwner))
                return await LoadOwnerDashboard();

            if (roles.Contains(AppRoles.Tenant))
                return await LoadTenantDashboard();

            if (IsStaffRole(roles))
                return await LoadStaffDashboard();

            TempData[AppMessages.ErrorMessage] = AppMessages.NoDashboardPermission;
            return RedirectToAction("AccessDenied", "Home");
        }


        /// <summary>
        /// Loads the SuperAdmin / Treasurer enhanced dashboard with global stats,
        /// apartment portfolio, financial summary, and quick actions.
        /// </summary>
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

        /// <summary>
        /// Loads the Manager dashboard scoped to their assigned apartment,
        /// including pending residents and notice board items.
        /// </summary>
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

        /// <summary>
        /// Loads the Community Leader dashboard (President, Secretary, or Treasurer)
        /// scoped to the leader's assigned apartment.
        /// Treasurer also receives apartment financial summary.
        /// </summary>
        private async Task<IActionResult> LoadCommunityLeaderDashboard()
        {
            try
            {
                var response = await EnhancedDashboardApi.GetCommunityLeaderDashboardAsync();

                if (response?.Success == true && response.Data != null)
                    return View("CommunityLeaderDashboard",
                        CommunityLeaderDashboardViewModelMapper.From(response.Data));

                TempData[AppMessages.ErrorMessage] = response?.Message ?? AppMessages.CommunityDashFailed;
                return View("Error");
            }
            catch (Exception ex)
            {
                TempData[AppMessages.ErrorMessage] = $"{AppMessages.CommunityDashFailed}: {ex.Message}";
                return View("Error");
            }
        }

        /// <summary>
        /// Loads the Resident Owner dashboard showing all owned flats and tenant info.
        /// </summary>
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

        /// <summary>
        /// Loads the Tenant dashboard showing the tenant's single assigned flat.
        /// </summary>
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

        /// <summary>
        /// Loads the Staff dashboard showing shift times and task placeholders.
        /// </summary>
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

        /// <summary>
        /// Checks whether any of the user's roles belongs to the staff role set.
        /// Uses <see cref="AppRoles.StaffRoles"/> for the authoritative role list.
        /// </summary>
        /// <param name="roles">Role names from the authenticated user's claims.</param>
        /// <returns>True if any role matches a staff role.</returns>
        private static bool IsStaffRole(IEnumerable<string> roles) =>
            roles.Any(r => AppRoles.StaffRoles.Contains(r));
    }
}



























/*
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

*/






















