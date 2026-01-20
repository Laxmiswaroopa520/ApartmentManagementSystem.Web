namespace ApartmentManagementSystem.Web.Services.DTOs.Onboarding
{
    public class FloorDto
    {
        public Guid Id { get; set; }
         public string Name { get; set; } = string.Empty;
        // public string Name => $"Floor {FloorNumber}";
        public int FloorNumber { get; set; }
    }
}
