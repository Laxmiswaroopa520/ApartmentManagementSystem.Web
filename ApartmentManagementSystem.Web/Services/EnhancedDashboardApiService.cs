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
            "api/EnhancedDashboard/admin"
        );
    }

    public async Task<ApiResponse<ManagerDashboardDto>?> GetManagerDashboardAsync()
    {
        return await ApiClient.GetAsync<ApiResponse<ManagerDashboardDto>>(
            "api/EnhancedDashboard/manager"                             // cahnged this line from "api/EnhancedDashboard/manager
        );
    }

    public async Task<ApiResponse<CommunityLeaderDashboardDto>?> GetCommunityLeaderDashboardAsync()
    {
        return await ApiClient.GetAsync<ApiResponse<CommunityLeaderDashboardDto>>(
            "api/EnhancedDashboard/community-leader"
        );
    }

    public async Task<ApiResponse<StaffDashboardDto>?> GetStaffDashboardAsync()
    {
        return await ApiClient.GetAsync<ApiResponse<StaffDashboardDto>>(
            "api/EnhancedDashboard/staff"
        );
    }

    public async Task<ApiResponse<AdvancedDashboardStatsDto>?> GetAdvancedDashboardStatsAsync()
    {
        return await ApiClient.GetAsync<ApiResponse<AdvancedDashboardStatsDto>>(
            "api/EnhancedDashboard/advanced-stats"
        );
    }

    public async Task<ApiResponse<ApartmentDashboardStatsDto>?> GetApartmentDashboardStatsAsync(Guid apartmentId)
    {
        return await ApiClient.GetAsync<ApiResponse<ApartmentDashboardStatsDto>>(
            $"api/EnhancedDashboard/apartment-stats/{apartmentId}"
        );
    }

    public async Task<ApiResponse<FinancialSummaryDto>?> GetFinancialSummaryAsync()
    {
        return await ApiClient.GetAsync<ApiResponse<FinancialSummaryDto>>(
            "api/EnhancedDashboard/financial-summary"
        );
    }

    public async Task<ApiResponse<FinancialSummaryDto>?> GetApartmentFinancialSummaryAsync(Guid apartmentId)
    {
        return await ApiClient.GetAsync<ApiResponse<FinancialSummaryDto>>(
            $"api/EnhancedDashboard/apartment-financial-summary/{apartmentId}"
        );
    }

    public async Task<ApiResponse<List<NoticeBoardMessageDto>>?> GetNoticeBoardMessagesAsync(Guid apartmentId)
    {
        return await ApiClient.GetAsync<ApiResponse<List<NoticeBoardMessageDto>>>(
            $"api/EnhancedDashboard/notice-board/{apartmentId}"
        );
    }

    public async Task<ApiResponse<List<QuickActionDto>>?> GetQuickActionsAsync()
    {
        return await ApiClient.GetAsync<ApiResponse<List<QuickActionDto>>>(
            "api/EnhancedDashboard/quick-actions"
        );
    }
}









/*using ApartmentManagementSystem.Web.Services.DTOs;

using ApartmentManagementSystem.Web.Services.DTOs.Dashboard;
using System.Threading;
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
*/














