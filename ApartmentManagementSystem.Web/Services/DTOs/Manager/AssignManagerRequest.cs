namespace ApartmentManagementSystem.Web.Services.DTOs.Manager
{
    public class AssignManagerRequest
    {
        public Guid ApartmentId { get; set; }

        //  pick an existing resident who has Manager role
        public Guid? UserId { get; set; }

        // External flow: create a new user on the fly
        public bool IsExternalManager { get; set; } = false;
        public string? ExternalManagerName { get; set; }
        public string? ExternalManagerPhone { get; set; }
        public string? ExternalManagerEmail { get; set; }

        //  Does this manager live in this apartment?
        public bool LivesInApartment { get; set; } = false;
    }

}