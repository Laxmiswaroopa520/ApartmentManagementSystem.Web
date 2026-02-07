using ApartmentManagementSystem.Web.ViewModels.Admin;

namespace ApartmentManagementSystem.Web.ViewModels.Dashboard
{
    public class ManagerDashboardViewModel
    {
        public string FullName { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public Guid ApartmentId { get; set; }
        public string ApartmentName { get; set; } = string.Empty;
        public ApartmentDashboardStatsViewModel Stats { get; set; } = new();
        public List<RecentActivityViewModel> RecentActivities { get; set; } = new();
        public List<QuickActionViewModel> QuickActions { get; set; } = new();
        public List<NoticeBoardMessageViewModel> NoticeBoard { get; set; } = new();
        public List<PendingResidentViewModel> PendingResidents { get; set; } = new();
    }
}