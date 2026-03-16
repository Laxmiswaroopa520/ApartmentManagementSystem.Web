/*namespace ApartmentManagementSystem.Web.Constants
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

        public const string AdminAndManager =
            SuperAdmin + "," + Manager;

        public const string AdminManagerCommunity =
            SuperAdmin + "," + Manager + "," +
            President + "," + Secretary + "," + Treasurer;
    }
}*/


namespace ApartmentManagementSystem.Web.Constants
{
    /// <summary>
    /// Centralised role name constants used for [Authorize] attributes,
    /// role checks in controllers, and role-based UI decisions.
    ///
    /// These must exactly match the role names seeded in the database
    /// and the claims added during JWT token generation.
    /// </summary>
    public static class AppRoles
    {
        //  System Roles
        public const string SuperAdmin = "SuperAdmin";
        public const string Manager = "Manager";

        // Community Roles
        public const string President = "President";
        public const string Secretary = "Secretary";
        public const string Treasurer = "Treasurer";

        //Resident Roles
        public const string ResidentOwner = "ResidentOwner";
        public const string Tenant = "Tenant";

        // Staff Roles 
        public const string Security = "Security";
        public const string Plumber = "Plumber";
        public const string Electrician = "Electrician";
        public const string Carpenter = "Carpenter";
        public const string Sweeper = "Sweeper";
        public const string Gardener = "Gardener";
        public const string MaintenanceStaff = "MaintenanceStaff";


        /// <summary>SuperAdmin only.</summary>
        public const string AdminOnly = SuperAdmin;

        /// <summary>SuperAdmin and Manager.</summary>
        public const string AdminAndManager = $"{SuperAdmin},{Manager}";

        /// <summary>SuperAdmin, Manager, President, Secretary, Treasurer.</summary>
        public const string AdminManagerCommunity =
            $"{SuperAdmin},{Manager},{President},{Secretary},{Treasurer}";

        /// <summary>All resident and admin roles.</summary>
        public const string AllResidentRoles =
            $"{SuperAdmin},{Manager},{ResidentOwner},{Tenant}";

        // Community Role List 
        /// <summary>
        /// The three elected community roles.
        /// Used for validation when checking if all community positions are filled.
        /// </summary>
        public static readonly IReadOnlyList<string> CommunityRoles = new[]
        {
            President,
            Secretary,
            Treasurer
        };

        // Staff Role List 
        /// <summary>
        /// All staff role names.
        /// Used to identify staff users for dashboard routing.
        /// </summary>
        public static readonly IReadOnlyList<string> StaffRoles = new[]
        {
            Security,
            Plumber,
            Electrician,
            Carpenter,
            Sweeper,
            Gardener,
            MaintenanceStaff
        };
    }
}