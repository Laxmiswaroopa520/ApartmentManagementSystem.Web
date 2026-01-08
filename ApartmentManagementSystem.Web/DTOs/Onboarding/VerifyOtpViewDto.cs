namespace ApartmentManagementSystem.Web.DTOs.Onboarding
{
    public class VerifyOtpViewDto
    {
        public string Email { get; set; } = string.Empty;
        public string Otp { get; set; } = string.Empty;
    }
}