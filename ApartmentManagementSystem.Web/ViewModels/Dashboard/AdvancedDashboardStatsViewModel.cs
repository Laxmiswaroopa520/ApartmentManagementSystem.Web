namespace ApartmentManagementSystem.Web.ViewModels.Dashboard
{
    public class AdvancedDashboardStatsViewModel
    {
        public int TotalResidents { get; set; }
        public int TotalFlats { get; set; }
        public int OccupiedFlats { get; set; }
        public int VacantFlats { get; set; }
        public int PendingRegistrations { get; set; }

        // Staff Statistics
        public int TotalStaffMembers { get; set; }
        public int ActiveStaffMembers { get; set; }
        public int CommunityMembers { get; set; }

        // Complaints Statistics
        public int PendingComplaints { get; set; }
        public int ResolvedComplaintsThisMonth { get; set; }

        // Financial Statistics
        public decimal TotalOutstandingBills { get; set; }
        public decimal CollectionThisMonth { get; set; }

        // Visitor Statistics
        public int TodaysVisitors { get; set; }
        public int ActiveSecurityPersonnel { get; set; }
    }
}