namespace ApartmentManagementSystem.Web.Services.DTOs.Admin
{
    public class AssignFlatResponse
    {
        public Guid UserId { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string FlatNumber { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
    }
}
