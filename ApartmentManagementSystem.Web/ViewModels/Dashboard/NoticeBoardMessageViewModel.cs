namespace ApartmentManagementSystem.Web.ViewModels.Dashboard
{
    public class NoticeBoardMessageViewModel
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public string Priority { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public string PostedBy { get; set; } = string.Empty;
        public string FlatNumber { get; set; } = string.Empty;
        public DateTime PostedAt { get; set; }
        public bool IsResolved { get; set; }
    }
}