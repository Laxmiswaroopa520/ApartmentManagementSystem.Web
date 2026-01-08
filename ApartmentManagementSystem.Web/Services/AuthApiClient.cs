using ApartmentManagementSystem.Web.DTOs.Auth;
using ApartmentManagementSystem.Web.DTOs.Auth;
using System.Net.Http.Json;
namespace ApartmentManagementSystem.Web.Services
{
    public class AuthApiClient
    {
        private readonly HttpClient _http;

        public AuthApiClient(HttpClient http)
        {
            _http = http;
        }

        public async Task<LoginApiResponseDto?> LoginAsync(LoginViewDto viewDto)
        {
            var apiRequest = new
            {
                Email = viewDto.Email,
                Password = viewDto.Password
            };

            var response = await _http.PostAsJsonAsync(
                "api/auth/login",
                apiRequest);

            if (!response.IsSuccessStatusCode)
                return null;

            return await response.Content
                .ReadFromJsonAsync<LoginApiResponseDto>();
        }
    }
}