namespace ApartmentManagementSystem.Web.Constants
{
    /// <summary>
    /// All string constants used across controllers:
    /// TempData keys, cookie keys, claim types, API error codes,
    /// and every user-facing success/error/validation message.
    /// Use string.Format() for messages that contain {0} placeholders.
    /// </summary>
    public static class AppMessages
    {
        // TempData keys
        public const string SuccessMessage = "SuccessMessage";
        public const string ErrorMessage = "ErrorMessage";

        //TempData transfer keys (Onboarding flow)
        public const string TempVerifiedPhone = "VerifiedPhone";
        public const string TempFullName = "FullName";

        //Cookie keys
        public const string CookieAuthToken = "AuthToken";
        public const string CookieUserName = "UserName";
        public const string CookieUserRole = "UserRole";
        public const string CookieUserId = "UserId";

        // Custom claim type
        public const string ClaimUsername = "Username";

        //API error codes
        public const string ErrorCodeAccountInactive = "ACCOUNT_INACTIVE";

        // Generic 
        public const string GenericError = "An unexpected error occurred. Please try again.";

        // Authentication
        public const string LoginFailed = "Login failed. Please check your credentials.";
        public const string LogoutSuccess = "Logged out successfully";
        public const string WelcomeBack = "Welcome back, {0}!";
        public const string NoDashboardPermission = "You do not have permission to access the dashboard.";

        // Dashboard 
        public const string DashboardLoadFailed = "Failed to load dashboard";
        public const string ManagerDashFailed = "Failed to load manager dashboard";
        public const string CommunityDashFailed = "Failed to load community leader dashboard";
        public const string OwnerDashFailed = "Failed to load owner dashboard";
        public const string TenantDashFailed = "Failed to load tenant dashboard";
        public const string StaffDashFailed = "Failed to load staff dashboard";

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

        // Flat assignment
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

        //  Registration 
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