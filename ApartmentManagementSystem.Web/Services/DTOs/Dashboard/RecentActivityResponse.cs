namespace ApartmentManagementSystem.Web.Services.DTOs.Dashboard
{
    public class RecentActivityResponse
    {
        public string Activity { get; set; } = string.Empty;
        public DateTime Timestamp { get; set; }
        public string Type { get; set; } = string.Empty;
    }
}
