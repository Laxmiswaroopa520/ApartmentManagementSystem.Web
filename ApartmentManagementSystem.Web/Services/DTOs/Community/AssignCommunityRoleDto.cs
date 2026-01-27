namespace ApartmentManagementSystem.Web.Services.DTOs.Community
{
    public class AssignCommunityRoleDto
    {
        public Guid UserId { get; set; }
        public string CommunityRole { get; set; } = string.Empty; // President, Secretary, or Treasurer
    }
}