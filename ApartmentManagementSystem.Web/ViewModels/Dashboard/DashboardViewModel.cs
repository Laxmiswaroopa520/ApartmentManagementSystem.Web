namespace ApartmentManagementSystem.Web.ViewModels.Dashboard
{
   public class DashboardViewModel
    {
        public string FullName { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string? FlatNumber { get; set; }

        // For admin dashboard
        public int TotalResidents { get; set; }
        public int PendingRegistrations { get; set; }
        public int TotalFlats { get; set; }
        public int VacantFlats { get; set; }
        public int OccupiedFlats { get; set; }
    }
}
