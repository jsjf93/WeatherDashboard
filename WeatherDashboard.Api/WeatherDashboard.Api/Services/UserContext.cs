using System.Security.Claims;
using WeatherDashboard.Api.Data.Repositories;

namespace WeatherDashboard.Api.Services;

public class UserContext(IHttpContextAccessor httpContextAccessor, UserRepository userRepository) : IUserContext
{
    private string? _email;
    private Guid _userId;
    private bool _initialized;

    public string Email
    {
        get
        {
            EnsureInitialized();
            return _email ?? string.Empty;
        }
    }

    public Guid UserId
    {
        get
        {
            EnsureInitialized();
            return _userId;
        }
    }

    private void EnsureInitialized()
    {
        if (_initialized)
            return;

        var httpContext = httpContextAccessor.HttpContext;
        if (httpContext == null)
        {
            throw new InvalidOperationException("HttpContext is not available");
        }

        var claimsPrincipal = httpContext.User;
        
        if (!claimsPrincipal.Identity?.IsAuthenticated ?? true)
        {
            throw new InvalidOperationException("User is not authenticated");
        }

        var emailClaim = claimsPrincipal.FindFirst(ClaimTypes.Email) ??
                         claimsPrincipal.FindFirst("email") ??
                         claimsPrincipal.FindFirst("preferred_username");

        if (emailClaim == null)
        {
            throw new InvalidOperationException("Email claim not found in JWT token");
        }

        _email = emailClaim.Value;

        // Auto-create user on first access
        var user = userRepository.GetOrCreateUserByEmailAsync(_email).GetAwaiter().GetResult();
        _userId = user.Id;

        _initialized = true;
    }
}
