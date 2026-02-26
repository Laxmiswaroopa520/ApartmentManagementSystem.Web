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













