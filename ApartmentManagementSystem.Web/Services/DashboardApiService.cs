using ApartmentManagementSystem.Web.Services.DTOs;
using ApartmentManagementSystem.Web.Services.DTOs.Dashboard;

namespace ApartmentManagementSystem.Web.Services
{

    public class DashboardApiService
    {
        private readonly ApiClient _apiClient;

        public DashboardApiService(ApiClient apiClient)
        {
            _apiClient = apiClient;
        }

        public async Task<ApiResponse<AdminDashboardResponse>?> GetAdminDashboardAsync()
        {
            return await _apiClient.GetAsync<ApiResponse<AdminDashboardResponse>>(
                "api/DashboardApi/admin"
            );
        }

        public async Task<ApiResponse<OwnerDashboardResponse>?> GetOwnerDashboardAsync()
        {
            return await _apiClient.GetAsync<ApiResponse<OwnerDashboardResponse>>(
                "api/DashboardApi/owner"
            );
        }

        public async Task<ApiResponse<TenantDashboardResponse>?> GetTenantDashboardAsync()
        {
            return await _apiClient.GetAsync<ApiResponse<TenantDashboardResponse>>(
                "api/DashboardApi/tenant"
            );
        }

        public async Task<ApiResponse<DashboardStatsResponse>?> GetDashboardStatsAsync()
        {
            return await _apiClient.GetAsync<ApiResponse<DashboardStatsResponse>>(
                "api/DashboardApi/stats"
            );
        }
    }
}