namespace ApartmentManagementSystem.Web.Services.DTOs.Dashboard
{
   public class AdminDashboardResponse
    {
        public string FullName { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public DashboardStatsDto Stats { get; set; } = new();
        public List<RecentActivityDto> RecentActivities { get; set; } = new();//previously it is recentactivitydto
    }
}