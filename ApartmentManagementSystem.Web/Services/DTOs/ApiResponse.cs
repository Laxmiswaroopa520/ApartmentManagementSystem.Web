using ApartmentManagementSystem.Web.Services.DTOs;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ApartmentManagementSystem.Web.Services.DTOs
{
    // pace ApartmentManagementSystem.Web.Services;

    public class ApiResponse<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public T? Data { get; set; }
        public List<string>? Errors { get; set; }
    }
}