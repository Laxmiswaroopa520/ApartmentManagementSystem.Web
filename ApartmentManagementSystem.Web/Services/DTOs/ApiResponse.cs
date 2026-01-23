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
       // public List<string>? Errors { get; set; }
        public string? ErrorCode { get; set; }      //this messsaeg is added while checking the inactive users authentication


        public static ApiResponse<T> SuccessResponse(T data, string message = "")
        {
            return new ApiResponse<T>
            {
                Success = true,
                Data = data,
                Message = message
            };
        }
        public static ApiResponse<T> ErrorResponse(
        string message,
        string? errorCode = null)
        {
            return new ApiResponse<T>
            {
                Success = false,
                Message = message,
                ErrorCode = errorCode
            };
        }

    }
}