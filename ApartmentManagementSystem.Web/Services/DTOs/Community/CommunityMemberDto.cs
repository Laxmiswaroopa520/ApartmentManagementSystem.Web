namespace ApartmentManagementSystem.Web.Services.DTOs.Community
{
    //response dto(deserialized from the api)

    public class CommunityMemberDto
    {
        public Guid UserId { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string FlatNumber { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public DateTime AssignedOn { get; set; }
        public bool IsActive { get; set; }
    }
    /*public class CommunityMemberDto
    {
    public Guid UserId { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string FlatNumber { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
    public DateTime AssignedOn { get; set; }
    public bool IsActive { get; set; }
}*/
}
