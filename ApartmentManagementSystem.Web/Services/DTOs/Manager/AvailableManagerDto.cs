namespace ApartmentManagementSystem.Web.Services.DTOs.Manager
{
    public class AvailableManagerDto
    {
        public Guid UserId { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
    }
}