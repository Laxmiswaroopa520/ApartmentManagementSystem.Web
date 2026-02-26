// Web/Services/CommunityMemberApiService.cs
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

        // Get all community members (optionally filtered by apartment)
        public async Task<ApiResponse<List<CommunityMemberDto>>?> GetAllCommunityMembersAsync(Guid? apartmentId = null)
        {
            var endpoint = apartmentId.HasValue
                ? $"api/CommunityMembers?apartmentId={apartmentId.Value}"
                : "api/CommunityMembers";

            return await ApiClient.GetAsync<ApiResponse<List<CommunityMemberDto>>>(endpoint);
        }

        // Get eligible resident owners from a specific apartment
        public async Task<ApiResponse<List<ResidentListDto>>?> GetEligibleResidentsAsync(Guid apartmentId)
        {
            return await ApiClient.GetAsync<ApiResponse<List<ResidentListDto>>>(
                $"api/CommunityMembers/eligible-residents/{apartmentId}"
            );
        }

        // Assign community role
        public async Task<ApiResponse<CommunityMemberDto>?> AssignCommunityRoleAsync(AssignCommunityRoleRequest request)
        {
            return await ApiClient.PostAsync<AssignCommunityRoleRequest, ApiResponse<CommunityMemberDto>>(
                "api/CommunityMembers/assign-role",
                request
            );
        }

        // Remove community role
        public async Task<ApiResponse<bool>?> RemoveCommunityRoleAsync(RemoveCommunityRoleRequest request)
        {
            return await ApiClient.PostAsync<RemoveCommunityRoleRequest, ApiResponse<bool>>(
                "api/CommunityMembers/remove-role",
                request
            );
        }
    }
}
















/*
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

        /// <summary>
        /// ⭐ Now accepts optional apartmentId to filter by apartment
        /// </summary>
        public async Task<ApiResponse<List<CommunityMemberDto>>?> GetAllCommunityMembersAsync(Guid? apartmentId = null)
        {
            var url = apartmentId.HasValue
                ? $"api/CommunityMembersApi?apartmentId={apartmentId.Value}"
                : "api/CommunityMembersApi";

            return await ApiClient.GetAsync<ApiResponse<List<CommunityMemberDto>>>(url);
        }

        /// <summary>
        /// ⭐ Now requires apartmentId — eligible residents must belong to THIS apartment
        /// </summary>
        /// 
        //this method is from prevoius service
        public async Task<ApiResponse<List<ResidentListDto>>?> GetEligibleResidentsAsync()
        {
            return await ApiClient.GetAsync<ApiResponse<List<ResidentListDto>>>(
                "api/CommunityMembersApi/eligible-residents"
            );
        }
        //2 methods included
       public async Task<ApiResponse<List<ResidentListDto>>?> GetEligibleResidentsAsync(Guid apartmentId)
        {
            return await ApiClient.GetAsync<ApiResponse<List<ResidentListDto>>>(
                $"api/CommunityMembersApi/eligible-residents?apartmentId={apartmentId}"
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

*/








