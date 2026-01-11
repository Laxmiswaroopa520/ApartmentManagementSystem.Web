namespace ApartmentManagementSystem.Web.Services.DTOs.Onboarding
{
    public class VerifyOtpResponse
    {
        public Guid UserId { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
    }
}
