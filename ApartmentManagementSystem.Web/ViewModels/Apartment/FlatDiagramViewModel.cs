namespace ApartmentManagementSystem.Web.ViewModels.Apartment
{
    public class FlatDiagramViewModel
    {
        public Guid FlatId { get; set; }
        public string FlatNumber { get; set; } = string.Empty;
        public bool IsOccupied { get; set; }
        public string? OccupantName { get; set; }
        public string? OccupantType { get; set; }
        public string Status { get; set; } = "Vacant";
    }
}