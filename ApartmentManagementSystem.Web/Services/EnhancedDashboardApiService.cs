using ApartmentManagementSystem.Web.Services.DTOs;

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
    //newly aed for apartment
    // Application/Services/EnhancedDashboardService.cs - Add apartment stats method

   /* public async Task<EnhancedAdminDashboardDto> GetEnhancedAdminDashboardAsync(Guid userId)
    {
        var user = await UserRepo.GetByIdAsync(userId)
            ?? throw new Exception("User not found");

        var roles = user.UserRoles.Select(ur => ur.Role.Name).ToList();
        var stats = await GetAdvancedDashboardStatsAsync();

        // ⭐ Populate apartment stats for SuperAdmin
        if (roles.Contains("SuperAdmin"))
        {
            stats.TotalApartments = await ApartmentRepo.GetTotalCountAsync();

            var allApartments = await ApartmentRepo.GetAllAsync();
            stats.ActiveApartments = allApartments.Count(a => a.Status == ApartmentStatus.Active);
            stats.ApartmentsUnderConstruction = allApartments.Count(a => a.Status == ApartmentStatus.UnderConstruction);

            var apartmentsWithDetails = await ApartmentRepo.GetAllWithDetailsAsync();
            stats.TotalFloors = apartmentsWithDetails.Sum(a => a.TotalFloors);

            stats.TotalManagers = await _context.Set<ApartmentManager>()
                .CountAsync(m => m.IsActive);
        }

        return new EnhancedAdminDashboardDto
        {
            FullName = user.FullName,
            AllRoles = roles,
            Stats = stats,
            RecentActivities = new List<RecentActivityDto>
        {
            new RecentActivityDto
            {
                Activity = "System initialized",
                Timestamp = DateTime.UtcNow,
                Type = "System"
            }
        }
        };*/
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
