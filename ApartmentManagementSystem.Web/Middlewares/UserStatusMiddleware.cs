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











