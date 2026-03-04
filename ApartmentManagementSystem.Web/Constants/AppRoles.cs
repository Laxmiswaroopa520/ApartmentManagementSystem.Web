namespace ApartmentManagementSystem.Web.Constants
{
    /// <summary>
    /// All role name constants used in [Authorize] attributes and role checks.
    /// Eliminates magic role strings across controllers.
    /// </summary>
    public static class AppRoles
    {
        // Individual roles
        public const string SuperAdmin = "SuperAdmin";
        public const string Manager = "Manager";
        public const string President = "President";
        public const string Secretary = "Secretary";
        public const string Treasurer = "Treasurer";
        public const string ResidentOwner = "ResidentOwner";
        public const string Tenant = "Tenant";

        // Staff roles
        public const string Security = "Security";
        public const string Plumber = "Plumber";
        public const string Electrician = "Electrician";
        public const string Carpenter = "Carpenter";
        public const string Sweeper = "Sweeper";
        public const string Gardener = "Gardener";
        public const string MaintenanceStaff = "MaintenanceStaff";

        // Combined strings for [Authorize(Roles = ...)]
        public const string AdminAndManager =
            SuperAdmin + "," + Manager;

        public const string AdminManagerCommunity =
            SuperAdmin + "," + Manager + "," +
            President + "," + Secretary + "," + Treasurer;
    }
}