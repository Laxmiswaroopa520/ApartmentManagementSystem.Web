namespace ApartmentManagementSystem.Web.Services.DTOs.Dashboard
{
    public class FlatSummaryResponse
    {
        public Guid FlatId { get; set; }
        public string FlatNumber { get; set; } = string.Empty;
        public string ApartmentName { get; set; } = string.Empty;
        public string OwnerName { get; set; } = string.Empty;
        public string? TenantName { get; set; }
    }
}
