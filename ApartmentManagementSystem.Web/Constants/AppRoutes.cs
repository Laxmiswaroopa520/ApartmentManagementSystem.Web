namespace ApartmentManagementSystem.Web.Constants
{
    /// <summary>
    /// Centralizes route-related strings: controller names, action names,
    /// cookie keys, claim types, and TempData keys.
    /// Prevents magic strings scattered across controller redirects and cookie operations.
    /// </summary>
    public static class AppRoutes
    {
        // Controller names (without "Controller" suffix)
        public static class Controllers
        {
            public const string Dashboard = "Dashboard";
            public const string Login = "Login";
            public const string Home = "Home";
            public const string ApartmentBuilder = "ApartmentBuilder";
            public const string AdminResidents = "AdminResidents";
            public const string Community = "Community";
            public const string CommunityMembers = "CommunityMembers";
            public const string VerifyInvite = "VerifyInvite";
            public const string Onboarding = "Onboarding";
        }

        // Action names
        public static class Actions
        {
            public const string Index = "Index";
            public const string Pending = "Pending";
            public const string Details = "Details";
            public const string AssignFlat = "AssignFlat";
            public const string AssignRole = "AssignRole";
            public const string Inactive = "Inactive";
            public const string AccessDenied = "AccessDenied";
            public const string ManageApartments = "ManageApartments";
            public const string Error = "Error";
        }

        // View names
        public static class Views
        {
            public const string Index = "Index";
            public const string Error = "Error";
            public const string ManagerDashboard = "ManagerDashboard";
            public const string CommunityLeaderDash = "CommunityLeaderDashboard";
            public const string OwnerDashboard = "OwnerDashboard";
            public const string TenantDashboard = "TenantDashboard";
            public const string StaffDashboard = "StaffDashboard";
        }
    }

    /// <summary>
    /// Centralizes TempData key strings used across controllers.
    /// </summary>
    public static class TempDataKeys
    {
        public const string SuccessMessage = "SuccessMessage";
        public const string ErrorMessage = "ErrorMessage";
        public const string VerifiedPhone = "VerifiedPhone";
        public const string FullName = "FullName";
    }

    /// <summary>
    /// Centralizes cookie key strings used for authentication and session.
    /// </summary>
    public static class CookieKeys
    {
        public const string AuthToken = "AuthToken";
        public const string UserName = "UserName";
        public const string UserRole = "UserRole";
        public const string UserId = "UserId";
    }

    /// <summary>
    /// Centralizes custom claim type strings used during cookie sign-in.
    /// </summary>
    public static class AppClaimTypes
    {
        public const string Username = "Username";
    }

    /// <summary>
    /// Centralizes error code strings returned from the API layer.
    /// </summary>
    public static class ApiErrorCodes
    {
        public const string AccountInactive = "ACCOUNT_INACTIVE";
    }
}