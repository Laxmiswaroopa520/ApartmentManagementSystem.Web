using ApartmentManagementSystem.Web.Services.DTOs.Login;
using ApartmentManagementSystem.Web.Services.DTOs;
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
        //added for inactive users in our apartment
        public async Task<bool> IsUserActiveAsync(Guid userId)
        {
            var response = await ApiClient.GetAsync<bool>(
                $"api/AuthApi/users/{userId}/is-active");

            return response;
        }


    }
}