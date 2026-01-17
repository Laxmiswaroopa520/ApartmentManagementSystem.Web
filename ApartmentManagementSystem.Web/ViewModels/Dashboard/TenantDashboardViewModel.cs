namespace ApartmentManagementSystem.Web.ViewModels.Dashboard
{
    public class TenantDashboardViewModel
    {
        public string FullName { get; set; } = string.Empty;
        public Guid UserId { get; set; }
        public FlatSummaryViewModel? MyFlat { get; set; }
        public int PendingComplaints { get; set; }
        public decimal PendingRent { get; set; }
    }
}