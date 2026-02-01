/*namespace ApartmentManagementSystem.Web.Services.DTOs.Manager
{
    public class AssignManagerRequest
    {
        public Guid ApartmentId { get; set; }
        public Guid UserId { get; set; }
    }
}*//*
public class AssignManagerRequest
{
    public Guid ApartmentId { get; set; }

    // For internal (existing user) assignment
    public Guid? UserId { get; set; }

    // ⭐ NEW: For external manager
    public bool IsExternalManager { get; set; } = false;
    public string? ExternalManagerName { get; set; }
    public string? ExternalManagerPhone { get; set; }
    public string? ExternalManagerEmail { get; set; }

    // ⭐ NEW: Optional flag
    public bool LivesInApartment { get; set; } = false;
}
*/
namespace ApartmentManagementSystem.Web.Services.DTOs.Manager
{
    public class AssignManagerRequest
    {
        public Guid ApartmentId { get; set; }

        // Internal flow: pick an existing resident who has Manager role
        public Guid? UserId { get; set; }

        // External flow: create a new user on the fly
        public bool IsExternalManager { get; set; } = false;
        public string? ExternalManagerName { get; set; }
        public string? ExternalManagerPhone { get; set; }
        public string? ExternalManagerEmail { get; set; }

        // Optional flag — does this manager live in this apartment?
        public bool LivesInApartment { get; set; } = false;
    }

}