using ApartmentManagementSystem.Web.Services.DTOs;
using ApartmentManagementSystem.Web.Services.DTOs.Staff;

namespace ApartmentManagementSystem.Web.Services
{
   public class StaffMemberApiService
    {
        private readonly ApiClient ApiClient;

        public StaffMemberApiService(ApiClient apiClient)
        {
            ApiClient = apiClient;
        }

        public async Task<ApiResponse<List<StaffMemberDto>>?> GetAllStaffMembersAsync()
        {
            return await ApiClient.GetAsync<ApiResponse<List<StaffMemberDto>>>(
                "api/ApartmentStaff"
            );
        }

        public async Task<ApiResponse<List<StaffMemberDto>>?> GetStaffMembersByTypeAsync(string staffType)
        {
            return await ApiClient.GetAsync<ApiResponse<List<StaffMemberDto>>>(
                $"api/ApartmentStaff/by-type/{staffType}"
            );
        }

        public async Task<ApiResponse<StaffMemberDto>?> CreateStaffMemberAsync(CreateStaffMemberRequest request)
        {
            return await ApiClient.PostAsync<CreateStaffMemberRequest, ApiResponse<StaffMemberDto>>(
                "api/ApartmentStaff",
                request
            );
        }

        public async Task<ApiResponse<StaffMemberDto>?> UpdateStaffMemberAsync(UpdateStaffMemberRequest request)
        {
            return await ApiClient.PostAsync<UpdateStaffMemberRequest, ApiResponse<StaffMemberDto>>(
                "api/ApartmentStaff",
                request
            );
        }

        public async Task<ApiResponse<bool>?> DeactivateStaffMemberAsync(Guid staffId)
        {
            return await ApiClient.PostAsync<object, ApiResponse<bool>>(
                $"api/ApartmentStaff/{staffId}/deactivate",
                new { }
            );
        }

        public async Task<ApiResponse<bool>?> ActivateStaffMemberAsync(Guid staffId)
        {
            return await ApiClient.PostAsync<object, ApiResponse<bool>>(
                $"api/ApartmentStaff/{staffId}/activate",
                new { }
            );
        }

        public async Task<ApiResponse<StaffMemberDto>?> GetStaffMemberByIdAsync(Guid staffId)
        {
            return await ApiClient.GetAsync<ApiResponse<StaffMemberDto>>(
                $"api/ApartmentStaff/{staffId}"
            );
        }
    }

}
