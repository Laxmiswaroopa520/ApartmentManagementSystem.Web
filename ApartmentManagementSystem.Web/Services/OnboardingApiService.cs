using ApartmentManagementSystem.Web.Services.DTOs;
using ApartmentManagementSystem.Web.Services.DTOs.Onboarding;
using System.Net.Http;

namespace ApartmentManagementSystem.Web.Services
{
    public class OnboardingApiService
    {
        private readonly ApiClient ApiClient;

        public OnboardingApiService(ApiClient apiClient)
        {
            ApiClient = apiClient;
        }

        public async Task<ApiResponse<CreateInviteResponse>?> CreateInviteAsync(CreateInviteRequest request)
        {
            return await ApiClient.PostAsync<CreateInviteRequest, ApiResponse<CreateInviteResponse>>(
                "api/OnboardingApi/create-invite", request);
        }

        public async Task<ApiResponse<VerifyOtpResponse>?> VerifyOtpAsync(VerifyOtpRequest request)
        {
            return await ApiClient.PostAsync<VerifyOtpRequest, ApiResponse<VerifyOtpResponse>>(
                "api/OnboardingApi/verify-otp", request);
        }

        public async Task<ApiResponse<CompleteRegistrationResponse>?> CompleteRegistrationAsync(CompleteRegistrationRequest request)
        {
            return await ApiClient.PostAsync<CompleteRegistrationRequest, ApiResponse<CompleteRegistrationResponse>>(
                "api/OnboardingApi/complete-registration", request);
        }
        //added this method for resident type dto from the api
        //for loading resident types
        public async Task<ApiResponse<List<ResidentTypeDto>>?> GetResidentTypesAsync()
        {
            return await ApiClient.GetAsync<ApiResponse<List<ResidentTypeDto>>>(
                "api/OnboardingApi/resident-types");
        }

      /*  public async Task<ApiResponse<List<ResidentTypeDto>>?> GetResidentTypesAsync()
        {
            return await ApiClient.GetAsync<ApiResponse<List<ResidentTypeDto>>>(
                "api/onboarding/resident-types");
        }
      */

    }
}
























/*{
    public class OnboardingApiService
    {
        private readonly ApiClient _apiClient;

        public OnboardingApiService(ApiClient apiClient)
        {
            _apiClient = apiClient;
        }

        // -------------------------
        // CREATE INVITE
        // -------------------------
        public async Task<ApiResponse<CreateInviteResponse>?> CreateInviteAsync(
            CreateInviteRequest request)
        {
            return await _apiClient.PostAsync<
                CreateInviteRequest,
                ApiResponse<CreateInviteResponse>>(
                "api/OnboardingApi/create-invite",
                request
            );
        }

        // -------------------------
        // VERIFY OTP
        // -------------------------
        public async Task<ApiResponse<VerifyOtpResponse>?> VerifyOtpAsync(
            VerifyOtpRequest request)
        {
            return await _apiClient.PostAsync<
                VerifyOtpRequest,
                ApiResponse<VerifyOtpResponse>>(
                "api/OnboardingApi/verify-otp",
                request
            );
        }

        // -------------------------
        // COMPLETE REGISTRATION
        // -------------------------
        public async Task<ApiResponse<CompleteRegistrationResponse>?> CompleteRegistrationAsync(
            CompleteRegistrationRequest request)
        {
            return await _apiClient.PostAsync<
                CompleteRegistrationRequest,
                ApiResponse<CompleteRegistrationResponse>>(
                "api/OnboardingApi/complete-registration",
                request
            );
        }

        // -------------------------
        // GET AVAILABLE ROLES
        // -------------------------
        public async Task<List<RoleOption>> GetAvailableRolesAsync()
        {
            var response = await _apiClient.GetAsync<List<RoleOption>>(
                "api/OnboardingApi/roles"
            );

            return response ?? new List<RoleOption>();
        }

        // -------------------------
        // GET FLOORS
      
        public async Task<List<FloorDto>> GetFloorsAsync()
        {
            var response = await _apiClient.GetAsync<List<FloorDto>>(
                "api/OnboardingApi/floors"
            );

            return response ?? new List<FloorDto>();
        }


        // -------------------------
        // GET AVAILABLE FLATS BY FLOOR
        // -------------------------
        public async Task<List<FlatDto>> GetAvailableFlatsAsync(Guid floorId)
        {
            var response = await _apiClient.GetAsync<List<FlatDto>>(
                $"api/OnboardingApi/available-flats?floorId={floorId}"
            );

            return response ?? new List<FlatDto>();
        }
    }
}




*/






























