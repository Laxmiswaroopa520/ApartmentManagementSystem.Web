namespace ApartmentManagementSystem.Web.Services.DTOs.Apartment
{
    public class ApartmentDiagramDto
    {
        public Guid ApartmentId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;     //Added this as part of visualize 3d image in my apartments page..
        public int TotalFloors { get; set; }
        public List<FloorDiagramDto> Floors { get; set; } = new();
    }
}