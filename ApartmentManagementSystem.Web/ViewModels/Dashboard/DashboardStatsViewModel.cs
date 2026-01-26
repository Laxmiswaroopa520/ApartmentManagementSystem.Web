namespace ApartmentManagementSystem.Web.ViewModels.Dashboard
{
    public class DashboardStatsViewModel
    {
        public int TotalResidents { get; set; }
        public int TotalFlats { get; set; }
        public int OccupiedFlats { get; set; }
        public int VacantFlats { get; set; }
        public int PendingComplaints { get; set; }
        public int PendingBills { get; set; }
        public int TodaysVisitors { get; set; }
    }
}

//        // Resident Statistics
//        public int TotalResidents { get; set; }
//        public int PendingRegistrations { get; set; }
//        public int ActiveResidents { get; set; }
//        public int InactiveResidents { get; set; }

//        // Flat Statistics
//        public int TotalFlats { get; set; }
//        public int OccupiedFlats { get; set; }
//        public int VacantFlats { get; set; }
//        public decimal OccupancyRate => TotalFlats > 0
//            ? (decimal)OccupiedFlats / TotalFlats * 100
//            : 0;

//        // Staff Statistics
//        public int TotalStaffMembers { get; set; }
//        public int ActiveStaffMembers { get; set; }
//        public int InactiveStaffMembers { get; set; }

//        // Community Statistics
//        public int CommunityMembers { get; set; }
//        public bool HasPresident { get; set; }
//        public bool HasSecretary { get; set; }
//        public bool HasTreasurer { get; set; }

//        // Complaints & Maintenance
//        public int PendingComplaints { get; set; }
//        public int ResolvedComplaintsThisMonth { get; set; }
//        public int TotalComplaintsThisMonth { get; set; }

//        // Visitors & Security
//        public int TodaysVisitors { get; set; }
//        public int ThisWeekVisitors { get; set; }
//        public int ActiveSecurityPersonnel { get; set; }

//        // Financial
//        public decimal TotalOutstandingBills { get; set; }
//        public decimal CollectionThisMonth { get; set; }
//        public int PendingBills { get; set; }
//        public int PaidBillsThisMonth { get; set; }

//        // ⭐ NEW: Apartment Management Statistics (for SuperAdmin)
//        public int? TotalApartments { get; set; }
//        public int? ActiveApartments { get; set; }
//        public int? TotalFloors { get; set; }
//        public int? TotalManagers { get; set; }
//        public int? ApartmentsUnderConstruction { get; set; }
//    } 
//}