/*using ApartmentManagementSystem.Web.Services.DTOs;
using ApartmentManagementSystem.Web.Services.DTOs.Community;
namespace ApartmentManagementSystem.Web.Services
{
    public class ResidentManagementApiService
    {
        private readonly ApiClient ApiClient;

        public ResidentManagementApiService(ApiClient apiClient)
        {
            ApiClient = apiClient;
        }

        public async Task<ApiResponse<List<ResidentListDto>>> GetAllResidentsAsync()
        {
            return await ApiClient.GetAsync<ApiResponse<List<ResidentListDto>>>(
                "api/ResidentManagementApi"
            ) ?? ApiResponse<List<ResidentListDto>>
                .ErrorResponse("No response from server");
        }

        public async Task<ApiResponse<List<ResidentListDto>>> GetResidentsByTypeAsync(string residentType)
        {
            return await ApiClient.GetAsync<ApiResponse<List<ResidentListDto>>>(
                $"api/ResidentManagementApi/by-type/{residentType}"
            ) ?? ApiResponse<List<ResidentListDto>>
                .ErrorResponse("No response from server");
        }

        public async Task<ApiResponse<ResidentDetailDto>> GetResidentDetailAsync(Guid userId)
        {
            return await ApiClient.GetAsync<ApiResponse<ResidentDetailDto>>(
                $"api/ResidentManagementApi/{userId}"
            ) ?? ApiResponse<ResidentDetailDto>
                .ErrorResponse("Resident not found");
        }

        // ✅ MUST MATCH Func<Guid, Task<ApiResponse<bool>>>
        public async Task<ApiResponse<bool>> DeactivateResidentAsync(Guid userId)
        {
            return await ApiClient.PostAsync<object, ApiResponse<bool>>(
                $"api/ResidentManagementApi/{userId}/deactivate",
                new { }
            ) ?? ApiResponse<bool>.ErrorResponse("Failed to deactivate resident");
        }

        // ✅ MUST MATCH Func<Guid, Task<ApiResponse<bool>>>
        public async Task<ApiResponse<bool>> ActivateResidentAsync(Guid userId)
        {
            return await ApiClient.PostAsync<object, ApiResponse<bool>>(
                $"api/ResidentManagementApi/{userId}/activate",
                new { }
            ) ?? ApiResponse<bool>.ErrorResponse("Failed to activate resident");
        }
    }
}
*/
/*using ApartmentManagementSystem.Web.Services.DTOs;
using ApartmentManagementSystem.Web.Services.DTOs.Community;

namespace ApartmentManagementSystem.Web.Services
{
    public class ResidentManagementApiService
    {
        private readonly ApiClient _apiClient;

        public ResidentManagementApiService(ApiClient apiClient)
        {
            _apiClient = apiClient;
        }

        public async Task<ApiResponse<List<ResidentListDto>>> GetAllResidentsAsync()
        {
            return await _apiClient.GetAsync<ApiResponse<List<ResidentListDto>>>(
                "api/ResidentManagementApi"
            ) ?? ApiResponse<List<ResidentListDto>>
                .ErrorResponse("No response from server");
        }

        public async Task<ApiResponse<List<ResidentListDto>>> GetResidentsByTypeAsync(string residentType)
        {
            return await _apiClient.GetAsync<ApiResponse<List<ResidentListDto>>>(
                $"api/ResidentManagementApi/by-type/{residentType}"
            ) ?? ApiResponse<List<ResidentListDto>>
                .ErrorResponse("No response from server");
        }

        public async Task<ApiResponse<ResidentDetailDto>> GetResidentDetailAsync(Guid userId)
        {
            return await _apiClient.GetAsync<ApiResponse<ResidentDetailDto>>(
                $"api/ResidentManagementApi/{userId}"
            ) ?? ApiResponse<ResidentDetailDto>
                .ErrorResponse("Resident not found");
        }

        // ✔ EXACT return type expected by controller
        /* public async Task<ApiResponse<bool>> DeactivateResidentAsync(Guid userId)
         {
             return await _apiClient.PostAsync<object, ApiResponse<bool>>(
                 $"api/ResidentManagementApi/{userId}/deactivate",
                 new { }
             ) ?? ApiResponse<bool>
                 .ErrorResponse("Failed to deactivate resident");
         }

         // ✔ EXACT return type expected by controller
         public async Task<ApiResponse<bool>> ActivateResidentAsync(Guid userId)
         {
             return await _apiClient.PostAsync<object, ApiResponse<bool>>(
                 $"api/ResidentManagementApi/{userId}/activate",
                 new { }
             ) ?? ApiResponse<bool>
                 .ErrorResponse("Failed to activate resident");
         }-----
        public Task<ApiResponse<bool>> ActivateResidentAsync(Guid userId)
        {
            return _apiClient.PostAsync<object, ApiResponse<bool>>(
                $"api/ResidentManagementApi/{userId}/activate",
                new { }
            );
        }

        public Task<ApiResponse<bool>> DeactivateResidentAsync(Guid userId)
        {
            return _apiClient.PostAsync<object, ApiResponse<bool>>(
                $"api/ResidentManagementApi/{userId}/deactivate",
                new { }
            );
        }

    }
}

*/














using ApartmentManagementSystem.Web.Services.DTOs;
using ApartmentManagementSystem.Web.Services.DTOs.Community;

namespace ApartmentManagementSystem.Web.Services
{
    public class ResidentManagementApiService
    {
        private readonly ApiClient ApiClient;

        public ResidentManagementApiService(ApiClient apiClient)
        {
            ApiClient = apiClient;
        }

        public async Task<ApiResponse<List<ResidentListDto>>> GetAllResidentsAsync()
        {
            return await ApiClient.GetAsync<ApiResponse<List<ResidentListDto>>>(
                "api/ResidentManagementApi"
            ) ?? ApiResponse<List<ResidentListDto>>
                .ErrorResponse("No response from server");
        }

        public async Task<ApiResponse<List<ResidentListDto>>> GetResidentsByTypeAsync(string residentType)
        {
            return await ApiClient.GetAsync<ApiResponse<List<ResidentListDto>>>(
                $"api/ResidentManagementApi/by-type/{residentType}"
            ) ?? ApiResponse<List<ResidentListDto>>
                .ErrorResponse("No response from server");
        }

        public async Task<ApiResponse<ResidentDetailDto>> GetResidentDetailAsync(Guid userId)
        {
            return await ApiClient.GetAsync<ApiResponse<ResidentDetailDto>>(
                $"api/ResidentManagementApi/{userId}"
            ) ?? ApiResponse<ResidentDetailDto>
                .ErrorResponse("Resident not found");
        }

        public async Task<ApiResponse<bool>> DeactivateResidentAsync(Guid userId)
        {
            return await ApiClient.PostAsync<object, ApiResponse<bool>>(
                $"api/ResidentManagementApi/{userId}/deactivate",
                new { }
            ) ?? ApiResponse<bool>.ErrorResponse("Failed to deactivate resident");
        }

        public async Task<ApiResponse<bool>> ActivateResidentAsync(Guid userId)
        {
            return await ApiClient.PostAsync<object, ApiResponse<bool>>(
                $"api/ResidentManagementApi/{userId}/activate",
                new { }
            ) ?? ApiResponse<bool>.ErrorResponse("Failed to activate resident");
        }
    }
}



















/*using ApartmentManagementSystem.Web.Services.DTOs;
using ApartmentManagementSystem.Web.Services.DTOs.Community;
namespace ApartmentManagementSystem.Web.Services
{
    public  class ResidentManagementApiService
    {
        private readonly ApiClient ApiClient;

        public ResidentManagementApiService(ApiClient apiClient)
        {
            ApiClient = apiClient;
        }

        public async Task<ApiResponse<List<ResidentListDto>>?> GetAllResidentsAsync()
        {
            return await ApiClient.GetAsync<ApiResponse<List<ResidentListDto>>>(
                "api/ResidentManagementApi"
            );
        }

        public async Task<ApiResponse<List<ResidentListDto>>?> GetResidentsByTypeAsync(string residentType)
        {
            return await ApiClient.GetAsync<ApiResponse<List<ResidentListDto>>>(
                $"api/ResidentManagementApi/by-type/{residentType}"
            );
        }

        public async Task<ApiResponse<ResidentDetailDto>?> GetResidentDetailAsync(Guid userId)
        {
            return await ApiClient.GetAsync<ApiResponse<ResidentDetailDto>>(
                $"api/ResidentManagementApi/{userId}"
            );
        }

        public async Task<ApiResponse<bool>?> DeactivateResidentAsync(Guid userId)
        {
            return await ApiClient.PostAsync<object, ApiResponse<bool>>(
                $"api/ResidentManagementApi/{userId}/deactivate",
                new { }
            );
        }
        /*
                public async Task<ApiResponse<bool>?> ActivateResidentAsync(Guid userId)
                {
                    return await ApiClient.PostAsync<object, ApiResponse<bool>>(
                        $"api/ResidentManagementApi/{userId}/activate",
                        new { }
                    );
                }
        ----------
public async Task<ApiResponse<bool>> ActivateResidentAsync(Guid userId)
        {
            var response = await ApiClient.PostAsync<ApiResponse<bool>>(
                $"residents/{userId}/activate",
                null
            );

            return response ?? ApiResponse<bool>.ErrorResponse("No response from server");
        }

    }
}
*/