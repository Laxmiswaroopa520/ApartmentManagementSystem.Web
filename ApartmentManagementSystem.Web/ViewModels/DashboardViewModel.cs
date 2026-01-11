namespace ApartmentManagementSystem.Web.ViewModels
{
    public class DashboardViewModel
    {
        public string UserName { get; set; } = string.Empty;
        public string UserRole { get; set; } = string.Empty;
        public Guid UserId { get; set; }

        // Statistics (for future phases)
        public int TotalFlats { get; set; }
        public int PendingComplaints { get; set; }
        public int OverdueBills { get; set; }
        public int PendingVisitors { get; set; }
    }
}