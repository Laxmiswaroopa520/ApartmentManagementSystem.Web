namespace ApartmentManagementSystem.Web.ViewModels.Dashboard
{
    public class QuickActionViewModel
    {
        public string Title { get; set; } = string.Empty;
        public string Icon { get; set; } = string.Empty;
        public string Url { get; set; } = string.Empty;
        public string Color { get; set; } = string.Empty;
        public bool RequiresPermission { get; set; }
    }
}