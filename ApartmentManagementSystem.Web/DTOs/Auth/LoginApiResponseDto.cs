namespace ApartmentManagementSystem.Web.DTOs.Auth
{
    public class LoginApiResponseDto
    {
        public string Token { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public DateTime ExpiresAt { get; set; }
    }
}