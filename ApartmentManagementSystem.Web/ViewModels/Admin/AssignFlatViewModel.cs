using System.ComponentModel.DataAnnotations;

namespace ApartmentManagementSystem.Web.ViewModels.Admin
{
    public class AssignFlatViewModel
    {
        [Required]
        public Guid UserId { get; set; }

        public string UserName { get; set; } = string.Empty;

        // ⭐ NEW: Apartment selection
        [Required(ErrorMessage = "Please select an apartment")]
        public Guid? ApartmentId { get; set; }

        [Required(ErrorMessage = "Please select a floor")]
        public Guid? FloorId { get; set; }

        [Required(ErrorMessage = "Please select a flat")]
        public Guid? FlatId { get; set; }

        // ⭐ NEW: Dropdown data
        public List<ApartmentDropdownViewModel> Apartments { get; set; } = new();
        public List<FloorDropdownViewModel> Floors { get; set; } = new();
        public List<FlatOption> Flats { get; set; } = new();
    }

    public class ApartmentDropdownViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }

    public class FloorDropdownViewModel
    {
        public Guid Id { get; set; }
        public int FloorNumber { get; set; }
    }

    public class FlatOption
    {
        public Guid Id { get; set; }
        public string FlatNumber { get; set; } = string.Empty;
    }
}








/*
using System.ComponentModel.DataAnnotations;

namespace ApartmentManagementSystem.Web.ViewModels.Admin
{
  public  class AssignFlatViewModel


    {
        [Required]
        public Guid UserId { get; set; }

        [Required(ErrorMessage = "Please select a floor")]
        public Guid? FloorId { get; set; }

        [Required(ErrorMessage = "Please select a flat")]
        public Guid? FlatId { get; set; }

        public string UserName { get; set; } = string.Empty;
        public List<FloorDropdownViewModel> Floors { get; set; } = new();
        // public List<FloorOption>? Floors { get; set; }
        public List<FlatOption>? Flats { get; set; } = new();
    }
    public class FloorDropdownViewModel
    {
        public Guid Id { get; set; }
        // public string FloorNumber { get; set; } = string.Empty;
        public int FloorNumber { get; set; }
    }
  /*  public class FloorOption
    {
        public Guid Id { get; set; }
        public string FloorNumber { get; set; } = string.Empty;
    }
    ----
public class FlatOption
    {
        public Guid Id { get; set; }
        public string FlatNumber { get; set; } = string.Empty;
    }
}
*/