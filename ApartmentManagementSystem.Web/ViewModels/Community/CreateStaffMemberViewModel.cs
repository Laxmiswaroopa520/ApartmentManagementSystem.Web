using System.ComponentModel.DataAnnotations;

namespace ApartmentManagementSystem.Web.ViewModels.Community
{

    public class CreateStaffMemberViewModel
    {
        [Required]
        public string FullName { get; set; } = string.Empty;
        [Required] 
        public string Phone { get; set; } = string.Empty;
        public string? Email { get; set; }
        public string? Address { get; set; }
        [Required]
        public string StaffType { get; set; } = string.Empty;
        public string? Specialization { get; set; }
        public decimal? HourlyRate { get; set; }
        public bool CreateLoginAccess { get; set; }
        public string? Password { get; set; }
        public Guid? ApartmentId { get; set; }
    }
}







