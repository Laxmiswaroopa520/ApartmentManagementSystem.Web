using System.ComponentModel.DataAnnotations;

namespace ApartmentManagementSystem.Web.ViewModels.Community
{
    public class UpdateStaffMemberViewModel
    {
        public Guid StaffId { get; set; }
        [Required] public string FullName { get; set; } = string.Empty;
        [Required] public string Phone { get; set; } = string.Empty;
        public string? Email { get; set; }
        public string? Address { get; set; }
        public bool IsActive { get; set; }
        public string? Specialization { get; set; }
        public decimal? HourlyRate { get; set; }
        public Guid? ApartmentId { get; set; }
        public string? ApartmentName { get; set; }
    }
}

















