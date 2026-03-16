using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ApartmentManagementSystem.Web.Constants
{
    public class ErrorMessages
    {
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
    }
}
