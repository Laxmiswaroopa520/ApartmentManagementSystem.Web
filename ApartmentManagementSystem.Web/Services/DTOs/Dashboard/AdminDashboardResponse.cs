namespace ApartmentManagementSystem.Web.Services.DTOs.Dashboard
{
    public class AdminDashboardResponse
    {
        public string FullName { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public DashboardStatsResponse Stats { get; set; } = new();
        public List<RecentActivityResponse> RecentActivities { get; set; } = new();
    }
}