namespace ApartmentManagementSystem.Web.ViewModels.Dashboard
{
    public class QuickActionViewModel
    {
        public string Title { get; set; } = string.Empty;
        public string Icon { get; set; } = string.Empty;
        public string Url { get; set; } = string.Empty;
        public string Color { get; set; } = string.Empty;
        public bool RequiresPermission { get; set; }
        //public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
       // public string Icon { get; set; } = string.Empty;
        public string ActionUrl { get; set; } = string.Empty;
        public string ActionController { get; set; } = string.Empty;
        public string ActionMethod { get; set; } = string.Empty;
        public string BadgeColor { get; set; } = "primary";
        public int? BadgeCount { get; set; }
    }
}