namespace ApartmentManagementSystem.Web.ViewModels.Apartment
{
    public class ApartmentDiagramViewModel
    {
        public Guid ApartmentId { get; set; }
        public string Name { get; set; } = string.Empty;
        public int TotalFloors { get; set; }
        public List<FloorDiagramViewModel> Floors { get; set; } = new();
    }


}