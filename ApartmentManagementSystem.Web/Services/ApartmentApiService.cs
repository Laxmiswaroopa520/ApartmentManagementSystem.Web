//new service
using ApartmentManagementSystem.Web.Services.DTOs;
using ApartmentManagementSystem.Web.Services.DTOs.Apartment;

namespace ApartmentManagementSystem.Web.Services
{
    public class ApartmentApiService
    {
        private readonly ApiClient ApiClient;

        public ApartmentApiService(ApiClient apiClient)
        {
            ApiClient = apiClient;
        }

        public async Task<ApiResponse<CreateApartmentResponseDto>?> CreateApartmentAsync(CreateApartmentDto dto)
        {
            return await ApiClient.PostAsync<CreateApartmentDto, ApiResponse<CreateApartmentResponseDto>>(
                "api/ApartmentManagement/create",
                dto
            );
        }

        public async Task<ApiResponse<List<ApartmentListDto>>?> GetAllApartmentsAsync()
        {
            return await ApiClient.GetAsync<ApiResponse<List<ApartmentListDto>>>(
                "api/ApartmentManagement/all"
            );
        }

        public async Task<ApiResponse<ApartmentDetailDto>?> GetApartmentDetailAsync(Guid apartmentId)
        {
            return await ApiClient.GetAsync<ApiResponse<ApartmentDetailDto>>(
                $"api/ApartmentManagement/{apartmentId}"
            );
        }

        public async Task<ApiResponse<ApartmentDiagramDto>?> GetApartmentDiagramAsync(Guid apartmentId)
        {
            return await ApiClient.GetAsync<ApiResponse<ApartmentDiagramDto>>(
                $"api/ApartmentManagement/{apartmentId}/diagram"
            );
        }
    }
}