using System.ComponentModel.DataAnnotations;

namespace ApartmentManagementSystem.Web.ViewModels.Admin
{
    public class AssignFlatViewModel
    {
        [Required]
        public Guid UserId { get; set; }

        public string UserName { get; set; } = string.Empty;

        // Apartment selection
        [Required(ErrorMessage = "Please select an apartment")]
        public Guid? ApartmentId { get; set; }

        [Required(ErrorMessage = "Please select a floor")]
        public Guid? FloorId { get; set; }

        [Required(ErrorMessage = "Please select a flat")]
        public Guid? FlatId { get; set; }

        //  Dropdown data
        public List<ApartmentDropdownViewModel> Apartments { get; set; } = new();
        public List<FloorDropdownViewModel> Floors { get; set; } = new();
        public List<FlatOption> Flats { get; set; } = new();
    }
}







