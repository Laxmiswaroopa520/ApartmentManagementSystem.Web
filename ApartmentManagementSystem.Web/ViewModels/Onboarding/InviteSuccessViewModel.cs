namespace ApartmentManagementSystem.Web.ViewModels.Onboarding
{
    public class InviteSuccessViewModel
    {
        public string FullName { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string OtpCode { get; set; } = string.Empty;
        public string ResidentType { get; set; } = string.Empty;
        public DateTime GeneratedAt { get; set; } = DateTime.Now;
        public DateTime ExpiresAt => GeneratedAt.AddMinutes(10);
        public string PrimaryPhone { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;

    }
}