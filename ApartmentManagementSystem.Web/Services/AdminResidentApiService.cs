using ApartmentManagementSystem.Web.Services.DTOs;
using ApartmentManagementSystem.Web.Services.DTOs.Admin;
using ApartmentManagementSystem.Web.Services.DTOs.Onboarding;
using ApartmentManagementSystem.Web.Services.DTOs.Apartment;

namespace ApartmentManagementSystem.Web.Services
{
    public class AdminResidentApiService
    {
        private readonly ApiClient ApiClient;

        public AdminResidentApiService(ApiClient apiClient)
        {
            ApiClient = apiClient;
        }

        public async Task<ApiResponse<List<PendingResidentDto>>?> GetPendingResidentsAsync()
        {
            return await ApiClient.GetAsync<ApiResponse<List<PendingResidentDto>>>(
                "api/AdminResidentApi/pending");
        }

        // ⭐ NEW: Get apartments for current user
        public async Task<ApiResponse<List<ApartmentDropdownDto>>?> GetApartmentsAsync()
        {
            return await ApiClient.GetAsync<ApiResponse<List<ApartmentDropdownDto>>>(
                "api/AdminResidentApi/apartments");
        }

        // ⭐ NEW: Get floors by apartment
        public async Task<ApiResponse<List<FloorDto>>?> GetFloorsByApartmentAsync(Guid apartmentId)
        {
            return await ApiClient.GetAsync<ApiResponse<List<FloorDto>>>(
                $"api/AdminResidentApi/apartments/{apartmentId}/floors");
        }

        public async Task<ApiResponse<List<FlatDto>>?> GetVacantFlatsByFloorAsync(Guid floorId)
        {
            return await ApiClient.GetAsync<ApiResponse<List<FlatDto>>>(
                $"api/AdminResidentApi/floors/{floorId}/flats");
        }

        public async Task<ApiResponse<AssignFlatResponse>?> AssignFlatAsync(AssignFlatRequest request)
        {
            return await ApiClient.PostAsync<AssignFlatRequest, ApiResponse<AssignFlatResponse>>(
                "api/AdminResidentApi/assign-flat", request);
        }
    }
}










