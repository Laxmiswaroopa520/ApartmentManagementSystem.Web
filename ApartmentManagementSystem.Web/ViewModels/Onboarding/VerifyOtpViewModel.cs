using System.ComponentModel.DataAnnotations;

namespace ApartmentManagementSystem.Web.ViewModels.Onboarding
{

    public class VerifyOtpViewModel
    {
        [Required(ErrorMessage = "Phone number is required")]
        [Phone(ErrorMessage = "Invalid phone number")]
        [Display(Name = "Phone Number")]
        [StringLength(10, MinimumLength = 10, ErrorMessage = "Phone number must be 10 digits")]
        public string PrimaryPhone { get; set; } = string.Empty;

        [Required(ErrorMessage = "OTP is required")]
        [Display(Name = "OTP Code")]
        [StringLength(6, MinimumLength = 6, ErrorMessage = "OTP must be 6 digits")]
        [RegularExpression(@"^\d{6}$", ErrorMessage = "OTP must be exactly 6 digits")]
        public string OtpCode { get; set; } = string.Empty;
    }
}