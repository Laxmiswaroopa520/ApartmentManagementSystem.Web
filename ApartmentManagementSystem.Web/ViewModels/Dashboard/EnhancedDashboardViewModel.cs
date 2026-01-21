namespace ApartmentManagementSystem.Web.ViewModels.Dashboard
{
    public class EnhancedDashboardViewModel
    {
        public string FullName { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public List<string> AllRoles { get; set; } = new();
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string? FlatNumber { get; set; }

        // Advanced Statistics
        public AdvancedDashboardStatsViewModel Stats { get; set; } = new();

        // Activities and Actions
        public List<RecentActivityViewModel> RecentActivities { get; set; } = new();
        public List<QuickActionViewModel> QuickActions { get; set; } = new();
        public List<UpcomingEventViewModel> UpcomingEvents { get; set; } = new();

        // Financial Summary (for SuperAdmin & Treasurer only)
        public FinancialSummaryViewModel? FinancialSummary { get; set; }
    }
}