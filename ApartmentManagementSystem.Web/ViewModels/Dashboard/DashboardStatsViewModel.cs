namespace ApartmentManagementSystem.Web.ViewModels.Dashboard
{
    public class DashboardStatsViewModel
    {
        public int TotalResidents { get; set; }
        public int TotalFlats { get; set; }
        public int OccupiedFlats { get; set; }
        public int VacantFlats { get; set; }
        public int PendingComplaints { get; set; }
        public int PendingBills { get; set; }
        public int TodaysVisitors { get; set; }
    }
}