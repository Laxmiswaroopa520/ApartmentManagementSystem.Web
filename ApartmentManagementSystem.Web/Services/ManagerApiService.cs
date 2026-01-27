using ApartmentManagementSystem.Web.Services.DTOs;
using ApartmentManagementSystem.Web.Services.DTOs.Admin;
using ApartmentManagementSystem.Web.Services.DTOs.Manager;

namespace ApartmentManagementSystem.Web.Services
{
    public class ManagerApiService
    {
        private readonly ApiClient _apiClient;

        public ManagerApiService(ApiClient apiClient)
        {
            _apiClient = apiClient;
        }

        public async Task<ApiResponse<List<AvailableManagerDto>>?> GetAvailableManagersAsync(Guid apartmentId)
        {
            return await _apiClient.GetAsync<ApiResponse<List<AvailableManagerDto>>>(
                $"api/Manager/available/{apartmentId}"
            );
        }

        public async Task<ApiResponse<ManagerAssignmentDto>?> AssignManagerToApartmentAsync(AssignManagerRequest request)
        {
            return await _apiClient.PostAsync<AssignManagerRequest, ApiResponse<ManagerAssignmentDto>>(
                "api/Manager/assign",
                request
            );
        }

        public async Task<ApiResponse<bool>?> RemoveManagerFromApartmentAsync(RemoveManagerRequest request)
        {
            return await _apiClient.PostAsync<RemoveManagerRequest, ApiResponse<bool>>(
                "api/Manager/remove",
                request
            );
        }
    }
}