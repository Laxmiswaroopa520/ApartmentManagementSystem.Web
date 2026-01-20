/*namespace ApartmentManagementSystem.Web.ViewModels.Onboarding
{
    using Microsoft.AspNetCore.Mvc.Rendering;
    using System.ComponentModel.DataAnnotations;

   // namespace ApartmentManagementSystem.Web.ViewModels.Onboarding;

    public class CompleteRegistrationViewModel
    {
        // Hidden field - passed from OTP verification
        [Required]
        public string PrimaryPhone { get; set; } = string.Empty;

        [Required(ErrorMessage = "Full name is required")]
        [Display(Name = "Full Name")]
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
        public string FullName { get; set; } = string.Empty;

        [Phone(ErrorMessage = "Invalid phone number")]
        [Display(Name = "Secondary Phone (Optional)")]
        [StringLength(10, MinimumLength = 10, ErrorMessage = "Phone number must be 10 digits")]
        public string? SecondaryPhone { get; set; }

        [EmailAddress(ErrorMessage = "Invalid email address")]
        [Display(Name = "Email Address (Optional)")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Username is required")]
        [Display(Name = "Username")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Username must be between 3 and 50 characters")]
        [RegularExpression(@"^[a-zA-Z0-9_]+$", ErrorMessage = "Username can only contain letters, numbers, and underscores")]
        public string Username { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password is required")]
        [Display(Name = "Password")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be at least 6 characters")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please confirm your password")]
        [Display(Name = "Confirm Password")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Passwords do not match")]
        public string ConfirmPassword { get; set; } = string.Empty;

        // ✅ NEW (ONLY FOR OWNER / TENANT)
        public Guid? FloorId { get; set; }
        public Guid? FlatId { get; set; }
        public Guid SelectedFloorId { get; set; }
        public Guid SelectedFlatId { get; set; }

        public List<SelectListItem> Floors { get; set; } = new();

    }
}
*/


using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ApartmentManagementSystem.Web.ViewModels.Onboarding
{
    public class CompleteRegistrationViewModel
    {
        // ===========================
        // Hidden / OTP info
        // ===========================
        [Required]
        public string PrimaryPhone { get; set; } = string.Empty;

        // ===========================
        // Personal info
        // ===========================
        [Required(ErrorMessage = "Full name is required")]
        [Display(Name = "Full Name")]
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
        public string FullName { get; set; } = string.Empty;

        [Phone(ErrorMessage = "Invalid phone number")]
        [Display(Name = "Secondary Phone (Optional)")]
        [StringLength(10, MinimumLength = 10, ErrorMessage = "Phone number must be 10 digits")]
        public string? SecondaryPhone { get; set; }

        [EmailAddress(ErrorMessage = "Invalid email address")]
        [Display(Name = "Email Address (Optional)")]
        public string? Email { get; set; }

        // ===========================
        // Login info
        // ===========================
        [Required(ErrorMessage = "Username is required")]
        [Display(Name = "Username")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Username must be between 3 and 50 characters")]
        [RegularExpression(@"^[a-zA-Z0-9_]+$", ErrorMessage = "Username can only contain letters, numbers, and underscores")]
        public string Username { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password is required")]
        [Display(Name = "Password")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be at least 6 characters")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please confirm your password")]
        [Display(Name = "Confirm Password")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Passwords do not match")]
        public string ConfirmPassword { get; set; } = string.Empty;

    }
}
