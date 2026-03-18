using Microsoft.AspNetCore.Http.HttpResults;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ApartmentManagementSystem.Web.Constants
{
    public class ErrorMessages
    {
       public const string OtpVerificationFailed= "OTP verification failed";
        public static readonly string ApartmentNotLoading = "Error loading apartments:";
        public const string ErrorLoadingDetails = "Error loading details";
        public const string ErrorLoadingDiagram = "Error loading diagram";
        
       public const string ErrorCreateApartment="Error in CreateApartment";
        public const string ErrorAssignManager = "Error assigning manager";
        public const string ErrorRemovingManager="Error removing manager:";
        public const string ErrorDeletingApartment = "Error deleting apartment";
        public const string ErrorLoadingCommunityMembers = "Error loading community members";
         public const string ErrorLoadingEligibleResidents = "Error loading eligible residents";
        public const string ErrorAssignRole="Error assigning role:";
        public const string ErrorRemovingRole = "Error removing role";
        public const string FailedToCreateInvite="Failed to create invite";
        //staff error messages
        public const string FailedToCreateStaffMessage = "Failed to create staff member";
        public const string FailedToUpdateStaff = "Failed to update staff member";
        public const string FailedToDeactivateStaff = "Failed to deactivate staff member";
        public const string FailedToActivateStaff="Failed to activate staff member";

    }
}
