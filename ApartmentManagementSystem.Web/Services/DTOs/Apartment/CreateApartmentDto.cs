namespace ApartmentManagementSystem.Web.Services.DTOs.Apartment
{
    public class CreateApartmentDto
    {
        public string Name { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string? City { get; set; }
        public string? State { get; set; }
        public string? PinCode { get; set; }
        public int TotalFloors { get; set; }
        public int FlatsPerFloor { get; set; }
    }
}