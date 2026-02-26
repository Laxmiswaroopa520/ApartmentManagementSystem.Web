namespace ApartmentManagementSystem.Web.ViewModels.Community
{
    public class AssignCommunityRoleViewModel
    {
        public Guid? ApartmentId { get; set; }      
        public Guid UserId { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string FlatNumber { get; set; } = string.Empty;
        public string CommunityRole { get; set; } = string.Empty;
        public List<string> AvailableRoles { get; set; } = new()
    {
        "President",
        "Secretary",
        "Treasurer"
    };
    }
}

