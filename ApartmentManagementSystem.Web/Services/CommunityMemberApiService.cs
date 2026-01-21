using ApartmentManagementSystem.Web.Services.DTOs;
using ApartmentManagementSystem.Web.Services.DTOs.Community;

namespace ApartmentManagementSystem.Web.Services
{
    public class CommunityMemberApiService
    {
        private readonly ApiClient _apiClient;

        public CommunityMemberApiService(ApiClient apiClient)
        {
            _apiClient = apiClient;
        }

        public async Task<ApiResponse<List<CommunityMemberDto>>?> GetAllCommunityMembersAsync()
        {
            return await _apiClient.GetAsync<ApiResponse<List<CommunityMemberDto>>>(
                "api/CommunityMembersApi"
            );
        }

        public async Task<ApiResponse<List<ResidentListDto>>?> GetEligibleResidentsAsync()
        {
            return await _apiClient.GetAsync<ApiResponse<List<ResidentListDto>>>(
                "api/CommunityMembersApi/eligible-residents"
            );
        }

        public async Task<ApiResponse<CommunityMemberDto>?> AssignCommunityRoleAsync(AssignCommunityRoleRequest request)
        {
            return await _apiClient.PostAsync<AssignCommunityRoleRequest, ApiResponse<CommunityMemberDto>>(
                "api/CommunityMembersApi/assign-role",
                request
            );
        }

        public async Task<ApiResponse<bool>?> RemoveCommunityRoleAsync(RemoveCommunityRoleRequest request)
        {
            return await _apiClient.PostAsync<RemoveCommunityRoleRequest, ApiResponse<bool>>(
                "api/CommunityMembersApi/remove-role",
                request
            );
        }

        public async Task<ApiResponse<CommunityMemberDto>?> GetCommunityMemberAsync(Guid userId)
        {
            return await _apiClient.GetAsync<ApiResponse<CommunityMemberDto>>(
                $"api/CommunityMembersApi/{userId}"
            );
        }
    }
}
