using ApartmentManagementSystem.Web.Services.DTOs;
using ApartmentManagementSystem.Web.Services.DTOs.Admin;
using ApartmentManagementSystem.Web.Services.DTOs.Onboarding;
using ApartmentManagementSystem.Web.Services.DTOs.Apartment;

namespace ApartmentManagementSystem.Web.Services
{
    public class AdminResidentApiService
    {
        private readonly ApiClient _apiClient;

        public AdminResidentApiService(ApiClient apiClient)
        {
            _apiClient = apiClient;
        }

        public async Task<ApiResponse<List<PendingResidentDto>>?> GetPendingResidentsAsync()
        {
            return await _apiClient.GetAsync<ApiResponse<List<PendingResidentDto>>>(
                "api/AdminResidentApi/pending");
        }

        // ⭐ NEW: Get apartments for current user
        public async Task<ApiResponse<List<ApartmentDropdownDto>>?> GetApartmentsAsync()
        {
            return await _apiClient.GetAsync<ApiResponse<List<ApartmentDropdownDto>>>(
                "api/AdminResidentApi/apartments");
        }

        // ⭐ NEW: Get floors by apartment
        public async Task<ApiResponse<List<FloorDto>>?> GetFloorsByApartmentAsync(Guid apartmentId)
        {
            return await _apiClient.GetAsync<ApiResponse<List<FloorDto>>>(
                $"api/AdminResidentApi/apartments/{apartmentId}/floors");
        }

        public async Task<ApiResponse<List<FlatDto>>?> GetVacantFlatsByFloorAsync(Guid floorId)
        {
            return await _apiClient.GetAsync<ApiResponse<List<FlatDto>>>(
                $"api/AdminResidentApi/floors/{floorId}/flats");
        }

        public async Task<ApiResponse<AssignFlatResponse>?> AssignFlatAsync(AssignFlatRequest request)
        {
            return await _apiClient.PostAsync<AssignFlatRequest, ApiResponse<AssignFlatResponse>>(
                "api/AdminResidentApi/assign-flat", request);
        }
    }
}











/*
namespace ApartmentManagementSystem.Web.Services
{
    public class AdminResidentApiService
    {
        private readonly ApiClient Apiclient;

        public AdminResidentApiService(ApiClient apiClient)
        {
            Apiclient = apiClient;
        }

        public async Task<ApiResponse<List<PendingResidentDto>>?> GetPendingResidentsAsync()
        {
            return await Apiclient.GetAsync<ApiResponse<List<PendingResidentDto>>>(
                "api/AdminResidentApi/pending");
        }

        public async Task<ApiResponse<AssignFlatResponse>?> AssignFlatAsync(AssignFlatRequest request)
        {
            return await Apiclient.PostAsync<AssignFlatRequest, ApiResponse<AssignFlatResponse>>(
                "api/AdminResidentApi/assign-flat", request);
        }

        public async Task<ApiResponse<List<FloorDto>>?> GetFloorsAsync()
        {
            return await Apiclient.GetAsync<ApiResponse<List<FloorDto>>>(
                "api/AdminResidentApi/floors");  
        }

        public async Task<ApiResponse<List<FlatDto>>?> GetVacantFlatsByFloorAsync(Guid floorId)
        {
            return await Apiclient.GetAsync<ApiResponse<List<FlatDto>>>(
                $"api/AdminResidentApi/floors/{floorId}/flats"); 
        }


       /* public async Task<ApiResponse<List<FloorDto>>?> GetFloorsAsync()
        {
            return await Apiclient.GetAsync<ApiResponse<List<FloorDto>>>(
                "api/FloorApi/all");
        }

        public async Task<ApiResponse<List<FlatDto>>?> GetVacantFlatsByFloorAsync(Guid floorId)
        {
            return await Apiclient.GetAsync<ApiResponse<List<FlatDto>>>(
                $"api/FlatApi/vacant-by-floor/{floorId}");
        }----
    }
}
*/