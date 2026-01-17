namespace ApartmentManagementSystem.Web.ViewModels.Dashboard
{
    public class OwnerDashboardViewModel
    {
        public string FullName { get; set; } = string.Empty;
        public Guid UserId { get; set; }
        public List<FlatSummaryViewModel> MyFlats { get; set; } = new();
        public int PendingComplaints { get; set; }
        public int PendingBills { get; set; }
        public decimal TotalOutstanding { get; set; }
    }
}