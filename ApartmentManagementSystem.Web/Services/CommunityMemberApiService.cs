using ApartmentManagementSystem.Web.Services.DTOs;
using ApartmentManagementSystem.Web.Services.DTOs.Community;

namespace ApartmentManagementSystem.Web.Services
{
    public class CommunityMemberApiService
    {
        private readonly ApiClient ApiClient;

        public CommunityMemberApiService(ApiClient apiClient)
        {
            ApiClient = apiClient;
        }

        public async Task<ApiResponse<List<CommunityMemberDto>>?> GetAllCommunityMembersAsync()
        {
            return await ApiClient.GetAsync<ApiResponse<List<CommunityMemberDto>>>(
                "api/CommunityMembersApi"
            );
        }

        public async Task<ApiResponse<List<ResidentListDto>>?> GetEligibleResidentsAsync()
        {
            return await ApiClient.GetAsync<ApiResponse<List<ResidentListDto>>>(
                "api/CommunityMembersApi/eligible-residents"
            );
        }

        public async Task<ApiResponse<CommunityMemberDto>?> AssignCommunityRoleAsync(AssignCommunityRoleRequest request)
        {
            return await ApiClient.PostAsync<AssignCommunityRoleRequest, ApiResponse<CommunityMemberDto>>(
                "api/CommunityMembersApi/assign-role",
                request
            );
        }

        public async Task<ApiResponse<bool>?> RemoveCommunityRoleAsync(RemoveCommunityRoleRequest request)
        {
            return await ApiClient.PostAsync<RemoveCommunityRoleRequest, ApiResponse<bool>>(
                "api/CommunityMembersApi/remove-role",
                request
            );
        }

        public async Task<ApiResponse<CommunityMemberDto>?> GetCommunityMemberAsync(Guid userId)
        {
            return await ApiClient.GetAsync<ApiResponse<CommunityMemberDto>>(
                $"api/CommunityMembersApi/{userId}"
            );
        }
    }
}
