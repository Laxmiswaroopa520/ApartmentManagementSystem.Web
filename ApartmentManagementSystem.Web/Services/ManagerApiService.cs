// ============================================================
// Web/Services/ManagerApiService.cs
// REPLACE your existing file completely.
// ============================================================
/* New One
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
        /// All users with Manager role (not already assigned to this apartment)
        /// </summary>
        public async Task<ApiResponse<List<AvailableManagerDto>>?> GetAvailableManagersAsync(Guid apartmentId)
        {
            return await _apiClient.GetAsync<ApiResponse<List<AvailableManagerDto>>>(
                $"api/Manager/available/{apartmentId}"
            );
        }

        /// <summary>
        /// GET /api/Manager/resident-managers/{apartmentId}
        /// Only Manager-role users who are residents of THIS apartment
        /// </summary>
        public async Task<ApiResponse<List<AvailableManagerDto>>?> GetResidentManagersAsync(Guid apartmentId)
        {
            return await _apiClient.GetAsync<ApiResponse<List<AvailableManagerDto>>>(
                $"api/Manager/resident-managers/{apartmentId}"
            );
        }

        /// <summary>
        /// POST /api/Manager/assign
        /// Forwards the full AssignManagerRequest (including external manager fields)
        /// </summary>
        public async Task<ApiResponse<ManagerAssignmentDto>?> AssignManagerToApartmentAsync(AssignManagerRequest request)
        {
            return await _apiClient.PostAsync<AssignManagerRequest, ApiResponse<ManagerAssignmentDto>>(
                "api/Manager/assign",
                request
            );
        }

        /// <summary>
        /// POST /api/Manager/remove
        /// </summary>
        public async Task<ApiResponse<bool>?> RemoveManagerFromApartmentAsync(RemoveManagerRequest request)
        {
            return await _apiClient.PostAsync<RemoveManagerRequest, ApiResponse<bool>>(
                "api/Manager/remove",
                request
            );
        }
    }
}

*/


// ============================================================
// PLACE AT: Web/Services/ManagerApiService.cs
// ACTION:   REPLACE your existing file completely
// ============================================================

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

        /// <summary>
        /// POST /api/Manager/assign
        /// Sends the FULL AssignManagerRequest — all 7 fields.
        /// The API-side ManagerApiController binds this to AssignManagerRequestDto
        /// which has the same shape, so all fields survive the round-trip.
        /// </summary>
        public async Task<ApiResponse<ManagerAssignmentDto>?> AssignManagerToApartmentAsync(AssignManagerRequest request)
        {
            return await _apiClient.PostAsync<AssignManagerRequest, ApiResponse<ManagerAssignmentDto>>(
                "api/Manager/assign",
                request
            );
        }

        /// <summary>
        /// POST /api/Manager/remove
        /// </summary>
        public async Task<ApiResponse<bool>?> RemoveManagerFromApartmentAsync(RemoveManagerRequest request)
        {
            return await _apiClient.PostAsync<RemoveManagerRequest, ApiResponse<bool>>(
                "api/Manager/remove",
                request
            );
        }
    }
}














/*using ApartmentManagementSystem.Web.Services.DTOs;
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
        /// ⭐ Get ALL available managers (route matches controller: /api/Manager/available/{id})
        /// </summary>
        public async Task<ApiResponse<List<AvailableManagerDto>>?> GetAvailableManagersAsync(Guid apartmentId)
        {
            return await _apiClient.GetAsync<ApiResponse<List<AvailableManagerDto>>>(
                $"api/Manager/available/{apartmentId}"
            );
        }

        /// <summary>
        /// ⭐ NEW: Get only managers who are residents of this apartment
        /// </summary>
        public async Task<ApiResponse<List<AvailableManagerDto>>?> GetResidentManagersAsync(Guid apartmentId)
        {
            return await _apiClient.GetAsync<ApiResponse<List<AvailableManagerDto>>>(
                $"api/Manager/resident-managers/{apartmentId}"
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
}*/

/*Main one 
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

*/