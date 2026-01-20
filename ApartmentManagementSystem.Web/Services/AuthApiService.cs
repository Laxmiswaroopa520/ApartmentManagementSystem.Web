using ApartmentManagementSystem.Web.Services.DTOs.Login;
using ApartmentManagementSystem.Web.Services.DTOs;
//using Microsoft.AspNetCore.Identity.Data;
//using Microsoft.AspNetCore.Identity;
//using ApartmentManagementSystem.Web.Services.DTOs.Login;
namespace ApartmentManagementSystem.Web.Services
{

    public class AuthApiService
    {
        private readonly ApiClient ApiClient;

        public AuthApiService(ApiClient apiClient)
        {
            ApiClient = apiClient;
        }

         public async Task<ApiResponse<LoginResponse>?> LoginAsync(LoginRequest request)
         {
             return await ApiClient.PostAsync<LoginRequest, ApiResponse<LoginResponse>>(
                 "api/AuthApi/login",
                 request
             );
         }

    }
}