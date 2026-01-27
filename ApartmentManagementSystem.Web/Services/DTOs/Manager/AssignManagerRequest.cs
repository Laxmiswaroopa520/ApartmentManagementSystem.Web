namespace ApartmentManagementSystem.Web.Services.DTOs.Manager
{
    public class AssignManagerRequest
    {
        public Guid ApartmentId { get; set; }
        public Guid UserId { get; set; }
    }
}