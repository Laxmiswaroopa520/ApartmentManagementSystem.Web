namespace ApartmentManagementSystem.Web.ViewModels.Apartment
{
    public class CommunityLeaderViewModel
    {
        public Guid UserId { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string FlatNumber { get; set; } = string.Empty;
        public DateTime AssignedAt { get; set; }
    }
}