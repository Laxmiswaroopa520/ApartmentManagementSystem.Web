using ApartmentManagementSystem.Web.Services.DTOs;
using ApartmentManagementSystem.Web.Services.DTOs.Onboarding;
using ApartmentManagementSystem.Web.ViewModels.Onboarding;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApartmentManagementSystem.Web.Services
{
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
        // -------------------------
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






















/*using ApartmentManagementSystem.Web.Services.DTOs;
using ApartmentManagementSystem.Web.Services.DTOs.Onboarding;
using ApartmentManagementSystem.Web.ViewModels.Onboarding;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApartmentManagementSystem.Web.Services
{
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
                "api/onboarding/create-invite",
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
                "api/onboarding/verify-otp",
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
                "api/onboarding/complete-registration",
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
        // -------------------------
        public async Task<List<FloorDto>> GetFloorsAsync()
        {
            var response = await _apiClient.GetAsync<List<FloorDto>>(
                "api/onboarding/floors"
            );
            return response ?? new List<FloorDto>();
        }

        // -------------------------
        // GET AVAILABLE FLATS BY FLOOR
        // -------------------------
        public async Task<List<FlatDto>> GetAvailableFlatsAsync(Guid floorId)
        {
            var response = await _apiClient.GetAsync<List<FlatDto>>(
                $"api/onboarding/available-flats?floorId={floorId}"
            );
            return response ?? new List<FlatDto>();
        }
    }
}

*/















/*using ApartmentManagementSystem.Web.Services.DTOs.Onboarding;
using ApartmentManagementSystem.Web.Services.DTOs;

namespace ApartmentManagementSystem.Web.Services;

public class OnboardingApiService
{
    private readonly ApiClient _apiClient;

    public OnboardingApiService(ApiClient apiClient)
    {
        _apiClient = apiClient;
    }

    public async Task<ApiResponse<CreateInviteResponse>?> CreateInviteAsync(CreateInviteRequest request)
    {
        return await _apiClient.PostAsync<CreateInviteRequest, ApiResponse<CreateInviteResponse>>(
            "api/OnboardingApi/create-invite",
            request
        );
    }

    public async Task<ApiResponse<VerifyOtpResponse>?> VerifyOtpAsync(VerifyOtpRequest request)
    {
        return await _apiClient.PostAsync<VerifyOtpRequest, ApiResponse<VerifyOtpResponse>>(
            "api/OnboardingApi/verify-otp",
            request
        );
    }

    public async Task<ApiResponse<CompleteRegistrationResponse>?> CompleteRegistrationAsync(CompleteRegistrationRequest request)
    {
        return await _apiClient.PostAsync<CompleteRegistrationRequest, ApiResponse<CompleteRegistrationResponse>>(
            "api/OnboardingApi/complete-registration",
            request
        );
    }
}
*/