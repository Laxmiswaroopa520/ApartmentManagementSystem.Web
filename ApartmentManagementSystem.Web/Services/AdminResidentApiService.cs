using ApartmentManagementSystem.Web.Services.DTOs;
using ApartmentManagementSystem.Web.Services.DTOs.Admin;
using ApartmentManagementSystem.Web.Services.DTOs.Onboarding;

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
        }*/
    }
}
