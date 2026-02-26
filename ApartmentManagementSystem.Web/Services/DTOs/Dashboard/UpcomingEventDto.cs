namespace ApartmentManagementSystem.Web.Services.DTOs.Dashboard
{
    public class UpcomingEventDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime EventDate { get; set; }
        public string EventType { get; set; } = string.Empty;
    }
}