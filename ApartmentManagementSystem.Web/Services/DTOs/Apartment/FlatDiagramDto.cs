namespace ApartmentManagementSystem.Web.Services.DTOs.Apartment
{
    public class FlatDiagramDto
    {
        public Guid FlatId { get; set; }
        public string FlatNumber { get; set; } = string.Empty;
        public bool IsOccupied { get; set; }
        public string? OccupantName { get; set; }
        public string? OccupantType { get; set; }
        public string Status { get; set; } = "Vacant";
    }
}