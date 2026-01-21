using ApartmentManagementSystem.Web.Services.DTOs;
using ApartmentManagementSystem.Web.Services.DTOs.Staff;

namespace ApartmentManagementSystem.Web.Services
{
   public class StaffMemberApiService
    {
        private readonly ApiClient _apiClient;

        public StaffMemberApiService(ApiClient apiClient)
        {
            _apiClient = apiClient;
        }

        public async Task<ApiResponse<List<StaffMemberDto>>?> GetAllStaffMembersAsync()
        {
            return await _apiClient.GetAsync<ApiResponse<List<StaffMemberDto>>>(
                "api/StaffMembersApi"
            );
        }

        public async Task<ApiResponse<List<StaffMemberDto>>?> GetStaffMembersByTypeAsync(string staffType)
        {
            return await _apiClient.GetAsync<ApiResponse<List<StaffMemberDto>>>(
                $"api/StaffMembersApi/by-type/{staffType}"
            );
        }

        public async Task<ApiResponse<StaffMemberDto>?> CreateStaffMemberAsync(CreateStaffMemberRequest request)
        {
            return await _apiClient.PostAsync<CreateStaffMemberRequest, ApiResponse<StaffMemberDto>>(
                "api/StaffMembersApi",
                request
            );
        }

        public async Task<ApiResponse<StaffMemberDto>?> UpdateStaffMemberAsync(UpdateStaffMemberRequest request)
        {
            return await _apiClient.PostAsync<UpdateStaffMemberRequest, ApiResponse<StaffMemberDto>>(
                "api/StaffMembersApi",
                request
            );
        }

        public async Task<ApiResponse<bool>?> DeactivateStaffMemberAsync(Guid staffId)
        {
            return await _apiClient.PostAsync<object, ApiResponse<bool>>(
                $"api/StaffMembersApi/{staffId}/deactivate",
                new { }
            );
        }

        public async Task<ApiResponse<bool>?> ActivateStaffMemberAsync(Guid staffId)
        {
            return await _apiClient.PostAsync<object, ApiResponse<bool>>(
                $"api/StaffMembersApi/{staffId}/activate",
                new { }
            );
        }

        public async Task<ApiResponse<StaffMemberDto>?> GetStaffMemberByIdAsync(Guid staffId)
        {
            return await _apiClient.GetAsync<ApiResponse<StaffMemberDto>>(
                $"api/StaffMembersApi/{staffId}"
            );
        }
    }

}
