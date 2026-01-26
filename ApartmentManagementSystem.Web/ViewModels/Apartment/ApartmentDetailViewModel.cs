namespace ApartmentManagementSystem.Web.ViewModels.Apartment
{
    public class ApartmentDetailViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string? City { get; set; }
        public string? State { get; set; }
        public string? PinCode { get; set; }
        public int TotalFloors { get; set; }
        public int FlatsPerFloor { get; set; }
        public int TotalFlats { get; set; }
        public int OccupiedFlats { get; set; }
        public int VacantFlats { get; set; }
        public string Status { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public ManagerInfoViewModel? Manager { get; set; }
        public CommunityLeaderViewModel? President { get; set; }
        public CommunityLeaderViewModel? Secretary { get; set; }
        public CommunityLeaderViewModel? Treasurer { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}