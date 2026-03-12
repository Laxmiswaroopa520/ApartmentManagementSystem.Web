// Web/Services/CommunityMemberApiService.cs
using ApartmentManagementSystem.Web.Services.DTOs;
using ApartmentManagementSystem.Web.Services.DTOs.Community;
using System.Net.Http.Json;

namespace ApartmentManagementSystem.Web.Services
{
    public class CommunityMemberApiService
    {
        private readonly HttpClient Http;

        public CommunityMemberApiService(HttpClient http)
        {
            Http = http;
        }

        // ── Apartments ──────────────────────────────────────────────
        // Calls the existing api/ApartmentManagement/all endpoint
        /*  public async Task<ApiResponse<List<ApartmentDto>>?> GetAllApartmentsAsync()
          {
              try
              {
                  return await Http.GetFromJsonAsync<ApiResponse<List<ApartmentDto>>>(
                      "api/ApartmentManagement/all");
              }
              catch { return null; }
          }*/
        public async Task<ApiResponse<List<ApartmentDto>>?> GetAllApartmentsAsync()
        {
            try
            {
                return await Http.GetFromJsonAsync<ApiResponse<List<ApartmentDto>>>(
                    "api/ApartmentManagement/active-list");
            }
            catch { return null; }
        }

        // ── Community Members ───────────────────────────────────────
        public async Task<ApiResponse<List<CommunityMemberDto>>?> GetAllCommunityMembersAsync(
            Guid? apartmentId = null)
        {
            var url = apartmentId.HasValue
                ? $"api/CommunityMembers?apartmentId={apartmentId}"
                : "api/CommunityMembers";
            try
            {
                return await Http.GetFromJsonAsync<ApiResponse<List<CommunityMemberDto>>>(url);
            }
            catch { return null; }
        }

        public async Task<ApiResponse<List<ResidentListDto>>?> GetEligibleResidentsAsync(
            Guid apartmentId)
        {
            try
            {
                return await Http.GetFromJsonAsync<ApiResponse<List<ResidentListDto>>>(
                    $"api/CommunityMembers/eligible-residents/{apartmentId}");
            }
            catch { return null; }
        }

        public async Task<ApiResponse<CommunityMemberDto>?> AssignCommunityRoleAsync(
            AssignCommunityRoleRequest request)
        {
            try
            {
                var response = await Http.PostAsJsonAsync(
                    "api/CommunityMembers/assign-role", request);
                return await response.Content
                    .ReadFromJsonAsync<ApiResponse<CommunityMemberDto>>();
            }
            catch { return null; }
        }

        public async Task<ApiResponse<bool>?> RemoveCommunityRoleAsync(
            RemoveCommunityRoleRequest request)
        {
            try
            {
                var response = await Http.PostAsJsonAsync(
                    "api/CommunityMembers/remove-role", request);
                return await response.Content.ReadFromJsonAsync<ApiResponse<bool>>();
            }
            catch { return null; }
        }

        // Helper: returns which roles are already assigned in an apartment
        public async Task<List<string>> GetAssignedRolesForApartmentAsync(Guid apartmentId)
        {
            var response = await GetAllCommunityMembersAsync(apartmentId);
            if (response?.Success == true && response.Data != null)
                return response.Data.Select(m => m.Role).Distinct().ToList();
            return new List<string>();
        }
    }
}



/*using ApartmentManagementSystem.Web.Services.DTOs;
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
*/















