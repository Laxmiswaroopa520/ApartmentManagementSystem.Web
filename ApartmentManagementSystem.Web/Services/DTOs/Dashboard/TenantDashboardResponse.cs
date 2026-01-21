namespace ApartmentManagementSystem.Web.Services.DTOs.Dashboard
{
    /*  public class TenantDashboardResponse
      {
          public string FullName { get; set; } = string.Empty;
          public Guid UserId { get; set; }
          public FlatSummaryResponse? MyFlat { get; set; }
          public int PendingComplaints { get; set; }
          public decimal PendingRent { get; set; }
    }  */
   public class TenantDashboardResponse
    {
        public string FullName { get; set; } = string.Empty;
        public Guid UserId { get; set; }
        public FlatSummaryDto? MyFlat { get; set; }
        public int PendingComplaints { get; set; }
        public decimal PendingRent { get; set; }
    }
}