namespace ApartmentManagementSystem.Web.Services.DTOs.Onboarding
{
    public class VerifyOtpRequest
    {
        public string PrimaryPhone { get; set; } = string.Empty;
        public string OtpCode { get; set; } = string.Empty;
    }
}
