namespace ApartmentManagementSystem.Web.Services.DTOs.Onboarding
{
    public class CreateUserInviteDto
    {
        public string FullName { get; set; } = string.Empty;
        public string PrimaryPhone { get; set; } = string.Empty;
        public int ResidentType { get; set; }  // 1=Owner,2=Tenant,3=Staff
    }
}
