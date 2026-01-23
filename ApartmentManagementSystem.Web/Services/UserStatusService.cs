using ApartmentManagementSystem.Web.Interface;

namespace ApartmentManagementSystem.Web.Services
{
    // used for checking user status whether active or inactive status..
    public class UserStatusService : IUserStatusService
    {
        private readonly ApiClient ApiClient;

        public UserStatusService(ApiClient apiClient)
        {
            ApiClient = apiClient;
        }

        public async Task<bool> IsUserActiveAsync(Guid userId)
        {
            var response = await ApiClient.GetAsync<bool>(
                $"api/users/{userId}/is-active");

            return response;
        }
    }
}