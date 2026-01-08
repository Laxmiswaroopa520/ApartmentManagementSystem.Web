using ApartmentManagementSystem.Web.ViewModels.Auth;

namespace ApartmentManagementSystem.Web.Services
{
    public class AuthApiService
    {
        private readonly HttpClient _client;

        public AuthApiService(IHttpClientFactory factory)
        {
            _client = factory.CreateClient("ApiClient");
        }

        public async Task<LoginResponseDto?> LoginAsync(LoginViewModel vm)
        {
            var response = await _client.PostAsJsonAsync(
                "api/auth/login",
                new LoginRequestDto
                {
                    Email = vm.Email,
                    Password = vm.Password
                });

            if (!response.IsSuccessStatusCode)
                return null;

            return await response.Content.ReadFromJsonAsync<LoginResponseDto>();
        }
    }
}