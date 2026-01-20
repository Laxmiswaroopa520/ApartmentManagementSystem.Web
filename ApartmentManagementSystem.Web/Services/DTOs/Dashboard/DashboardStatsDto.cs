namespace ApartmentManagementSystem.Web.Services.DTOs.Dashboard
{
   public class DashboardStatsDto
    {
        public int TotalResidents { get; set; }
        public int PendingRegistrations { get; set; }
        public int TotalFlats { get; set; }
        public int VacantFlats { get; set; }
    }
}
