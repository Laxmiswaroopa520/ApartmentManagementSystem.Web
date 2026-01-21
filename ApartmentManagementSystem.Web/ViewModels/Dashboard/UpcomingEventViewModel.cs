namespace ApartmentManagementSystem.Web.ViewModels.Dashboard
{
    public class UpcomingEventViewModel
    {
        public Guid EventId { get; set; }
        public string Title { get; set; } = string.Empty;
        public DateTime EventDate { get; set; }
        public string Type { get; set; } = string.Empty;
    }
}