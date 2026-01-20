using System.ComponentModel.DataAnnotations;

namespace ApartmentManagementSystem.Web.ViewModels.Onboarding
{

    /*  public class CreateInviteViewModel
      {
          [Required(ErrorMessage = "Full name is required")]
          [StringLength(100)]
          public string FullName { get; set; } = string.Empty;

          [Required(ErrorMessage = "Phone number is required")]
          [Phone(ErrorMessage = "Invalid phone number")]
          [StringLength(20)]
          public string PrimaryPhone { get; set; } = string.Empty;

          [Required(ErrorMessage = "Resident type is required")]
          public int ResidentType { get; set; } // 1=Owner, 2=Tenant, 3=Staff
          public string Message { get; set; } = string.Empty;
          public List<ResidentTypeOption>? ResidentTypes { get; set; }
      }
      */
    /*  public class CreateInviteViewModel
      {
          [Required(ErrorMessage = "Full name is required")]
          public string FullName { get; set; } = string.Empty;

          [Required(ErrorMessage = "Phone number is required")]
          public string PrimaryPhone { get; set; } = string.Empty;

          [Required(ErrorMessage = "Please select resident type")]
          [Range(1, 3, ErrorMessage = "Invalid resident type")]
          public int ResidentType { get; set; }

          public List<ResidentTypeOption>? ResidentTypes { get; set; }
      }
    */
    public class CreateInviteViewModel
    {
        [Required(ErrorMessage = "Full name is required")]
        public string FullName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Phone number is required")]
        public string PrimaryPhone { get; set; } = string.Empty;

        [Range(1, 3, ErrorMessage = "Please select a resident type")]
        public int ResidentType { get; set; }

        public List<ResidentTypeOption> ResidentTypes { get; set; } = new();
    }


    public class ResidentTypeOption
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }


}














    /*  public class CreateInviteViewModel
    {
    [Required(ErrorMessage = "Full name is required")]
    [Display(Name = "Full Name")]
    [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
    public string FullName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Phone number is required")]
    [Phone(ErrorMessage = "Invalid phone number")]
    [Display(Name = "Primary Phone")]
    [StringLength(10, MinimumLength = 10, ErrorMessage = "Phone number must be 10 digits")]
    [RegularExpression(@"^\d{10}$", ErrorMessage = "Phone number must be exactly 10 digits")]
    public string PrimaryPhone { get; set; } = string.Empty;

    [Required(ErrorMessage = "Role is required")]
    [Display(Name = "User Role")]
    public Guid RoleId { get; set; }

    // For dropdown binding
    public List<RoleOption>? AvailableRoles { get; set; }
}
  */

