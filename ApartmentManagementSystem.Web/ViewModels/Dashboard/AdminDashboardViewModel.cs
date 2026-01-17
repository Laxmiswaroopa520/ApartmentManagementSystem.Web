namespace ApartmentManagementSystem.Web.ViewModels.Dashboard
{
    public class AdminDashboardViewModel
    {
        public string FullName { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public DashboardStatsViewModel Stats { get; set; } = new();
        public List<RecentActivityViewModel> RecentActivities { get; set; } = new();
    }
}