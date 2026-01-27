namespace ApartmentManagementSystem.Web.Services.DTOs.Manager
{
    public class ManagerAssignmentDto
    {
        public Guid ApartmentId { get; set; }
        public Guid UserId { get; set; }
        public string FullName { get; set; } = string.Empty;
        public DateTime AssignedAt { get; set; }
    }
}