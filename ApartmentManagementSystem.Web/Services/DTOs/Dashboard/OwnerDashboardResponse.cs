namespace ApartmentManagementSystem.Web.Services.DTOs.Dashboard
{
    public class OwnerDashboardResponse
    {
        public string FullName { get; set; } = string.Empty;
        public Guid UserId { get; set; }
        public List<FlatSummaryDto> MyFlats { get; set; } = new();
        public int PendingComplaints { get; set; }
        public int PendingBills { get; set; }
        public decimal TotalOutstanding { get; set; }
    }
}