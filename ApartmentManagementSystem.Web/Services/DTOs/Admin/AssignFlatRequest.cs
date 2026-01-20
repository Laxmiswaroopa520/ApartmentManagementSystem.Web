namespace ApartmentManagementSystem.Web.Services.DTOs.Admin
{
    public class AssignFlatRequest
    {
        public Guid UserId { get; set; }
        public Guid FlatId { get; set; }
    }
}