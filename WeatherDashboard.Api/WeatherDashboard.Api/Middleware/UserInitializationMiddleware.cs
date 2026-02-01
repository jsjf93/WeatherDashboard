using System.Security.Claims;
using WeatherDashboard.Api.Data.Repositories;

namespace WeatherDashboard.Api.Middleware;

public class UserInitializationMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context, UserRepository userRepository)
    {
        var claimsPrincipal = context.User;

        if (claimsPrincipal.Identity?.IsAuthenticated ?? false)
        {
            var emailClaim = claimsPrincipal.FindFirst(ClaimTypes.Email) ??
                             claimsPrincipal.FindFirst("email") ??
                             claimsPrincipal.FindFirst("preferred_username");

            if (emailClaim != null)
            {
                try
                {
                    await userRepository.GetOrCreateUserByEmailAsync(emailClaim.Value);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Failed to initialize user: {ex.Message}");
                }
            }
        }

        await next(context);
    }
}
