namespace ApartmentManagementSystem.Web.DTOs.Onboarding
{
  //  namespace ApartmentManagementSystem.Web.DTOs.Onboarding;

    public class InviteUserViewDto
    {
        public string Email { get; set; } = string.Empty;
        public string Mobile { get; set; } = string.Empty;
        public Guid RoleId { get; set; }
    }
}