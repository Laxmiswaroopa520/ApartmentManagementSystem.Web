namespace ApartmentManagementSystem.Web.Constants
{
    /// <summary>
    /// Centralizes all user-facing messages used across controllers.
    /// Eliminates magic strings for success, error, and validation messages.
    /// </summary>
    public static class AppMessages
    {
        // Generic
        public const string GenericError = "An unexpected error occurred. Please try again.";
        public const string UnauthorizedAccess = "You don't have permission to access this resource.";

        // Authentication
        public const string LoginFailed = "Login failed. Please check your credentials.";
        public const string LogoutSuccess = "Logged out successfully";
        public const string AccountInactive = "Your account is inactive. Please contact support.";
        public const string WelcomeBack = "Welcome back, {0}!";

        // Dashboard
        public const string DashboardLoadFailed = "Failed to load dashboard";
        public const string ManagerDashboardFailed = "Failed to load manager dashboard";
        public const string CommunityDashFailed = "Failed to load community leader dashboard";
        public const string OwnerDashboardFailed = "Failed to load owner dashboard";
        public const string TenantDashboardFailed = "Failed to load tenant dashboard";
        public const string StaffDashboardFailed = "Failed to load staff dashboard";
        public const string NoDashboardPermission = "You don't have permission to access the dashboard.";

        // Apartment
        public const string ApartmentLoadFailed = "Failed to load apartments";
        public const string ApartmentDetailFailed = "Failed to load apartment details";
        public const string ApartmentNotFound = "Apartment not found";
        public const string ApartmentDiagramFailed = "Failed to load apartment diagram";
        public const string ApartmentDiagramNotFound = "Diagram not found";
        public const string ApartmentCreateSuccess = "Apartment created successfully!";
        public const string ApartmentCreateFailed = "Failed to create apartment";
        public const string ApartmentDeleteSuccess = "Apartment deleted successfully!";
        public const string ApartmentDeleteFailed = "Failed to delete apartment";
        public const string InvalidApartmentData = "Invalid data provided";

        // Manager
        public const string ManagerAssignSuccess = "Manager assigned successfully!";
        public const string ManagerAssignFailed = "Failed to assign manager";
        public const string ManagerRemoveSuccess = "Manager removed successfully!";
        public const string ManagerRemoveFailed = "Failed to remove manager";

        // Flat Assignment
        public const string FlatAssignSuccess = "Flat assigned successfully";
        public const string FlatAssignFailed = "Failed to assign flat";

        // Community
        public const string CommunityLoadFailed = "Failed to load community members";
        public const string CommunityRoleAssignSuccess = "{0} role assigned successfully!";
        public const string CommunityRoleAssignFailed = "Failed to assign role.";
        public const string CommunityRoleRemoveSuccess = "Community role removed successfully.";
        public const string CommunityRoleRemoveFailed = "Failed to remove role.";
        public const string EligibleResidentsLoadFailed = "Failed to load eligible residents";
        public const string ApartmentIdRequired = "Apartment ID is required to assign a community role.";
        public const string ApartmentIdRequiredShort = "Apartment ID is required";

        // Registration
        public const string OtpVerifyFirst = "Please verify OTP first";
        public const string RegistrationFailed = "Registration failed";

        // Validation
        public const string SelectApartment = "Please select an apartment.";
        public const string SelectFloor = "Please select a floor.";
        public const string SelectFlat = "Please select a flat.";
        public const string SelectResident = "Please select a resident.";
        public const string SelectRole = "Please select a role.";
        public const string ApartmentRequired = "Apartment is required.";
    }
}