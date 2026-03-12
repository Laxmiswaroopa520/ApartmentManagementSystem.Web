//for community Members Feature
namespace ApartmentManagementSystem.Web.Services.DTOs.Community
{
    public class ApartmentDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public bool IsActive { get; set; }
    }
}