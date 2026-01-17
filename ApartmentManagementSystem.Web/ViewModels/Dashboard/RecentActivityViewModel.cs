namespace ApartmentManagementSystem.Web.ViewModels.Dashboard
{
    public class RecentActivityViewModel
    {
        public string Activity { get; set; } = string.Empty;
        public DateTime Timestamp { get; set; }
        public string Type { get; set; } = string.Empty;
    }
}