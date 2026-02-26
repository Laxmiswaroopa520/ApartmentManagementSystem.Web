using ApartmentManagementSystem.Web.Services.DTOs;
using ApartmentManagementSystem.Web.Services.DTOs.Dashboard;

namespace ApartmentManagementSystem.Web.Services
{
    public class DashboardApiService
    {
        private readonly ApiClient ApiClient;

        public DashboardApiService(ApiClient apiClient)
        {
            ApiClient = apiClient;
        }

        public async Task<ApiResponse<AdminDashboardResponse>?> GetAdminDashboardAsync()
        {
            return await ApiClient.GetAsync<ApiResponse<AdminDashboardResponse>>(
                "api/DashboardApi/admin"
            );
        }

        public async Task<ApiResponse<OwnerDashboardResponse>?> GetOwnerDashboardAsync()
        {
            return await ApiClient.GetAsync<ApiResponse<OwnerDashboardResponse>>(
                "api/DashboardApi/owner"
            );
        }

        public async Task<ApiResponse<TenantDashboardResponse>?> GetTenantDashboardAsync()
        {
            return await ApiClient.GetAsync<ApiResponse<TenantDashboardResponse>>(
                "api/DashboardApi/tenant"
            );
        }
        public async Task<ApiResponse<DashboardStatsDto>?> GetDashboardStatsAsync()
        {
            return await ApiClient.GetAsync<ApiResponse<DashboardStatsDto>>(
                "api/DashboardApi/stats");
        }
    }
}