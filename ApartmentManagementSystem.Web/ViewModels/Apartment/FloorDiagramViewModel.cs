namespace ApartmentManagementSystem.Web.ViewModels.Apartment
{
    public class FloorDiagramViewModel
    {
        public Guid FloorId { get; set; }
        public int FloorNumber { get; set; }
        public string Name { get; set; } = string.Empty;
        public List<FlatDiagramViewModel> Flats { get; set; } = new();
    }
}