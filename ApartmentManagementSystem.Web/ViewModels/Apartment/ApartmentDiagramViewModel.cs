namespace ApartmentManagementSystem.Web.ViewModels.Apartment
{
    public class ApartmentDiagramViewModel
    {
        public Guid ApartmentId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty; //added this as part of visualize 3d-error
        public int TotalFloors { get; set; }
        public List<FloorDiagramViewModel> Floors { get; set; } = new();
    }


}