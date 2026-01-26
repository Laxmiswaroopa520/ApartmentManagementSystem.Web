//new service
using ApartmentManagementSystem.Web.Services.DTOs;
using ApartmentManagementSystem.Web.Services.DTOs.Apartment;

namespace ApartmentManagementSystem.Web.Services
{
    public class ApartmentApiService
    {
        private readonly ApiClient _apiClient;

        public ApartmentApiService(ApiClient apiClient)
        {
            _apiClient = apiClient;
        }

        public async Task<ApiResponse<CreateApartmentResponseDto>?> CreateApartmentAsync(CreateApartmentDto dto)
        {
            return await _apiClient.PostAsync<CreateApartmentDto, ApiResponse<CreateApartmentResponseDto>>(
                "api/ApartmentManagement/create",
                dto
            );
        }

        public async Task<ApiResponse<List<ApartmentListDto>>?> GetAllApartmentsAsync()
        {
            return await _apiClient.GetAsync<ApiResponse<List<ApartmentListDto>>>(
                "api/ApartmentManagement/all"
            );
        }

        public async Task<ApiResponse<ApartmentDetailDto>?> GetApartmentDetailAsync(Guid apartmentId)
        {
            return await _apiClient.GetAsync<ApiResponse<ApartmentDetailDto>>(
                $"api/ApartmentManagement/{apartmentId}"
            );
        }

        public async Task<ApiResponse<ApartmentDiagramDto>?> GetApartmentDiagramAsync(Guid apartmentId)
        {
            return await _apiClient.GetAsync<ApiResponse<ApartmentDiagramDto>>(
                $"api/ApartmentManagement/{apartmentId}/diagram"
            );
        }
    }
}