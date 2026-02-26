using ApartmentManagementSystem.Web.Services.DTOs;
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

        /// <summary>
        /// GET /api/Manager/available/{apartmentId}
        /// All users with Manager role, excluding whoever is already assigned to this apartment.
        /// </summary>
        public async Task<ApiResponse<List<AvailableManagerDto>>?> GetAvailableManagersAsync(Guid apartmentId)
        {
            return await _apiClient.GetAsync<ApiResponse<List<AvailableManagerDto>>>(
                $"api/Manager/available/{apartmentId}"
            );
        }

        /// <summary>
        /// GET /api/Manager/resident-managers/{apartmentId}
        /// Only Manager-role users who actually live in THIS apartment.
        /// This is what the "Resident Manager" tab dropdown loads.
        /// </summary>
        public async Task<ApiResponse<List<AvailableManagerDto>>?> GetResidentManagersAsync(Guid apartmentId)
        {
            return await _apiClient.GetAsync<ApiResponse<List<AvailableManagerDto>>>(
                $"api/Manager/resident-managers/{apartmentId}"
            );
        }

        /// POST /api/Manager/assign
        /// Sends the FULL AssignManagerRequest — all 7 fields.
        /// The API-side ManagerApiController binds this to AssignManagerRequestDto
        /// which has the same shape, so all fields survive the round-trip.
        public async Task<ApiResponse<ManagerAssignmentDto>?> AssignManagerToApartmentAsync(AssignManagerRequest request)
        {
            return await _apiClient.PostAsync<AssignManagerRequest, ApiResponse<ManagerAssignmentDto>>(
                "api/Manager/assign",
                request
            );
        }
        /// POST /api/Manager/remove
        public async Task<ApiResponse<bool>?> RemoveManagerFromApartmentAsync(RemoveManagerRequest request)
        {
            return await _apiClient.PostAsync<RemoveManagerRequest, ApiResponse<bool>>(
                "api/Manager/remove",
                request
            );
        }
        public async Task<ApiResponse<List<AvailableManagerDto>>?> GetApartmentResidentsAsync(Guid apartmentId)
        {
            return await _apiClient.GetAsync<ApiResponse<List<AvailableManagerDto>>>(
                $"api/Manager/apartment-residents/{apartmentId}"
            );
        }

    }
}













