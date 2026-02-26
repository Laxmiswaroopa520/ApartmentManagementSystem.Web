namespace ApartmentManagementSystem.Web.Services.DTOs.Dashboard
{
    public class RecentActivityDto
    {
        public string Activity { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public DateTime Timestamp { get; set; }
    }
}
