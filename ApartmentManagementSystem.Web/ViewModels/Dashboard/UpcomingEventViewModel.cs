namespace ApartmentManagementSystem.Web.ViewModels.Dashboard
{
    public class UpcomingEventViewModel
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime EventDate { get; set; }
        public string EventType { get; set; } = string.Empty;
    }
}