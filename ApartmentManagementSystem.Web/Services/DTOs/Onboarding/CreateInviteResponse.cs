
namespace ApartmentManagementSystem.Web.Services.DTOs.Onboarding
{
    public class CreateInviteResponse
    {
    public Guid UserId { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string PrimaryPhone { get; set; } = string.Empty;
    public string OtpCode { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
}
}
