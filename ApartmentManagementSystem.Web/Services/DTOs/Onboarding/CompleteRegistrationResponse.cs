/*namespace ApartmentManagementSystem.Web.Services.DTOs.Onboarding
{
    public class CompleteRegistrationResponse
    {
        public Guid UserId { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
    }
}*/
namespace ApartmentManagementSystem.Web.Services.DTOs.Onboarding;

public class CompleteRegistrationResponse
{
    public Guid UserId { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
}