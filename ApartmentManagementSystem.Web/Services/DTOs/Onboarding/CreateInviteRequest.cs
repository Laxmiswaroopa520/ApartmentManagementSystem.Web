namespace ApartmentManagementSystem.Web.Services.DTOs.Onboarding
{
    public class CreateInviteRequest
    {
    public string FullName { get; set; } = string.Empty;
    public string PrimaryPhone { get; set; } = string.Empty;
  //  public Guid RoleId { get; set; }
        public int ResidentType { get; set; }
    }
}
