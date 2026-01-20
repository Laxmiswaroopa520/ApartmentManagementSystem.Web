namespace ApartmentManagementSystem.Web.ViewModels
{
    public class ErrorViewModel
    {
        public string? RequestId { get; set; }
        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
        public string ErrorMessage { get; set; } = "An error occurred";
    }
}
