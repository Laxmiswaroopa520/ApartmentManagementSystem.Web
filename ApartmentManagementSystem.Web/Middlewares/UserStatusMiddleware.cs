using ApartmentManagementSystem.Web.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;

namespace ApartmentManagementSystem.Web.Middlewares;

public class UserStatusMiddleware
{
    private readonly RequestDelegate Next;

    public UserStatusMiddleware(RequestDelegate next)
    {
        Next = next;
    }

    public async Task InvokeAsync(HttpContext context, AuthApiService authApiService)
    {
        // Only check for authenticated users
        if (context.User.Identity?.IsAuthenticated == true)
        {
            var userIdClaim = context.User.FindFirst(ClaimTypes.NameIdentifier);

            if (userIdClaim != null &&
                Guid.TryParse(userIdClaim.Value, out var userId))
            {
                var isActive = await authApiService.IsUserActiveAsync(userId);

                if (!isActive)
                {
                    // Logout
                    await context.SignOutAsync(
                        CookieAuthenticationDefaults.AuthenticationScheme);

                    context.Response.Redirect("/Login/Inactive");
                    return;
                }
            }
        }

        await Next(context);
    }
}













/*using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using ApartmentManagementSystem.Web.Interface;
// this middle ware is used for checking the status of the user like if inactive users tries to login it has to show this page..
namespace ApartmentManagementSystem.Wen.Middlewares
{
    public class UserStatusMiddleware
    {
        private readonly RequestDelegate _next;

        public UserStatusMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, IUserStatusService userStatusService)
        {
            if (context.User.Identity?.IsAuthenticated == true)
            {
                var userIdClaim = context.User.FindFirst(ClaimTypes.NameIdentifier);

                if (userIdClaim != null &&
                    Guid.TryParse(userIdClaim.Value, out var userId))
                {
                    var isActive = await userStatusService.IsUserActiveAsync(userId);

                    if (!isActive)
                    {
                        // Logout user
                        await context.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

                        // Clear cookies
                        context.Response.Cookies.Delete("AuthToken");
                        context.Response.Cookies.Delete("UserName");
                        context.Response.Cookies.Delete("UserRole");
                        context.Response.Cookies.Delete("UserId");

                        context.Response.Redirect("/Login/Inactive");
                        return;
                    }
                }
            }

            await _next(context);
        }
    }
}
*/
