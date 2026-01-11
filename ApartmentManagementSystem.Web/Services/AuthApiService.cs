using ApartmentManagementSystem.Web.Services.DTOs.Login;
using ApartmentManagementSystem.Web.Services.DTOs;
//using Microsoft.AspNetCore.Identity.Data;
//using Microsoft.AspNetCore.Identity;
//using ApartmentManagementSystem.Web.Services.DTOs.Login;
namespace ApartmentManagementSystem.Web.Services
{

    public class AuthApiService
    {
        private readonly ApiClient _apiClient;

        public AuthApiService(ApiClient apiClient)
        {
            _apiClient = apiClient;
        }

         public async Task<ApiResponse<LoginResponse>?> LoginAsync(LoginRequest request)
         {
             return await _apiClient.PostAsync<LoginRequest, ApiResponse<LoginResponse>>(
                 "api/AuthApi/login",
                 request
             );
         }

    }
}