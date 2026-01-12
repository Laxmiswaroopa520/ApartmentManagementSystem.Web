using ApartmentManagementSystem.Web.Services.DTOs;
using ApartmentManagementSystem.Web.Services.DTOs.Onboarding;
using ApartmentManagementSystem.Web.ViewModels.Onboarding;

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
        // GET AVAILABLE ROLES (NEW)
        // -------------------------
        public async Task<List<RoleOption>> GetAvailableRolesAsync()
        {
            var response = await _apiClient.GetAsync<List<RoleOption>>(
                "api/roles"
            );

            return response ?? new List<RoleOption>();
        }
    }
}


















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