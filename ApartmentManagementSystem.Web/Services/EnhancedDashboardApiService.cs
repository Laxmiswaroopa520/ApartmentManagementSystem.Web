using ApartmentManagementSystem.Web.Services.DTOs;

using ApartmentManagementSystem.Web.Services.DTOs.Dashboard;
namespace ApartmentManagementSystem.Web.Services;

public class EnhancedDashboardApiService
{
    private readonly ApiClient ApiClient;

    public EnhancedDashboardApiService(ApiClient apiClient)
    {
        ApiClient = apiClient;
    }

    public async Task<ApiResponse<EnhancedAdminDashboardDto>?> GetEnhancedAdminDashboardAsync()
    {
        return await ApiClient.GetAsync<ApiResponse<EnhancedAdminDashboardDto>>(
            "api/EnhancedDashboardApi/admin"
        );
    }

    public async Task<ApiResponse<StaffDashboardDto>?> GetStaffDashboardAsync()
    {
        return await ApiClient.GetAsync<ApiResponse<StaffDashboardDto>>(
            "api/EnhancedDashboardApi/staff"
        );
    }

    public async Task<ApiResponse<AdvancedDashboardStatsDto>?> GetAdvancedDashboardStatsAsync()
    {
        return await ApiClient.GetAsync<ApiResponse<AdvancedDashboardStatsDto>>(
            "api/EnhancedDashboardApi/advanced-stats"
        );
    }

    public async Task<ApiResponse<FinancialSummaryDto>?> GetFinancialSummaryAsync()
    {
        return await ApiClient.GetAsync<ApiResponse<FinancialSummaryDto>>(
            "api/EnhancedDashboardApi/financial-summary"
        );
    }

    public async Task<ApiResponse<List<QuickActionDto>>?> GetQuickActionsAsync()
    {
        return await ApiClient.GetAsync<ApiResponse<List<QuickActionDto>>>(
            "api/EnhancedDashboardApi/quick-actions"
        );
    }
}
















/*
namespace ApartmentManagementSystem.Web.Services
{
    public class EnhancedDashboardApiService
    {
        private readonly ApiClient _apiClient;

        public EnhancedDashboardApiService(ApiClient apiClient)
        {
            _apiClient = apiClient;
        }

        public async Task<ApiResponse<EnhancedAdminDashboardDto>?> GetEnhancedAdminDashboardAsync()
        {
            return await _apiClient.GetAsync<ApiResponse<EnhancedAdminDashboardDto>>(
                "api/EnhancedDashboardApi/admin"
            );
        }

        public async Task<ApiResponse<StaffDashboardDto>?> GetStaffDashboardAsync()
        {
            return await _apiClient.GetAsync<ApiResponse<StaffDashboardDto>>(
                "api/EnhancedDashboardApi/staff"
            );
        }

        public async Task<ApiResponse<AdvancedDashboardStatsDto>?> GetAdvancedDashboardStatsAsync()
        {
            return await _apiClient.GetAsync<ApiResponse<AdvancedDashboardStatsDto>>(
                "api/EnhancedDashboardApi/advanced-stats"
            );
        }

        public async Task<ApiResponse<FinancialSummaryDto>?> GetFinancialSummaryAsync()
        {
            return await _apiClient.GetAsync<ApiResponse<FinancialSummaryDto>>(
                "api/EnhancedDashboardApi/financial-summary"
            );
        }

        public async Task<ApiResponse<List<QuickActionDto>>?> GetQuickActionsAsync()
        {
            return await _apiClient.GetAsync<ApiResponse<List<QuickActionDto>>>(
                "api/EnhancedDashboardApi/quick-actions"
            );
        }
    }
    */
