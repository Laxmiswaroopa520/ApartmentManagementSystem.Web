namespace ApartmentManagementSystem.Web.ViewModels.Community
{
    public class ResidentListViewModel
    {
        public Guid UserId { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string ResidentType { get; set; } = string.Empty;
        public string? FlatNumber { get; set; }
        public string? ApartmentName { get; set; }  //Added this for displaying apartment name in all residents Option..
        public string Status { get; set; } = string.Empty;
        public DateTime RegisteredOn { get; set; }
    }
}