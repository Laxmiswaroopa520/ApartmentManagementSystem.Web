namespace ApartmentManagementSystem.Web.Services.DTOs.Community
{
    public class AssignCommunityRoleRequest
    {
        public Guid UserId { get; set; }
        public string CommunityRole { get; set; } = string.Empty;
        public Guid? ApartmentId { get; set; }
    }

}
