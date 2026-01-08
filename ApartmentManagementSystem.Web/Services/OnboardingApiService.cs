using ApartmentManagementSystem.Web.ViewModels.Auth;

namespace ApartmentManagementSystem.Web.Services
{
    public class OnboardingApiService
    {
        private readonly ApiClient _api;

        public OnboardingApiService(ApiClient api)
        {
            _api = api;
        }

        public async Task VerifyOtpAsync(VerifyInviteViewModel vm)
        {
            await _api.PostAsync<object>(
                "/api/onboarding/verify-otp",
                vm
            );
        }

        public async Task CompleteRegistrationAsync(CompleteRegistrationViewModel vm)
        {
            await _api.PostAsync<object>(
                "/api/onboarding/complete-registration",
                vm
            );
        }
    }
}