using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Moq;
using System.Security.Claims;
using WeatherDashboard.Api.Data.Repositories;
using WeatherDashboard.Api.Services;

namespace WeatherDashboard.Api.Tests.Services;

public class UserContextTests
{
    private readonly Mock<IHttpContextAccessor> _httpContextAccessorMock;
    private readonly Mock<UserRepository> _userRepositoryMock;

    public UserContextTests()
    {
        _httpContextAccessorMock = new Mock<IHttpContextAccessor>();
        _userRepositoryMock = new Mock<UserRepository>(MockBehavior.Strict, null!);
    }

    [Fact]
    public void Email_WithNullHttpContext_ThrowsException()
    {
        // Arrange
        _httpContextAccessorMock.Setup(x => x.HttpContext).Returns((HttpContext)null!);
        var service = new UserContext(_httpContextAccessorMock.Object, _userRepositoryMock.Object);

        // Act
        var act = () => service.Email;

        // Assert
        act.Should().Throw<InvalidOperationException>()
            .WithMessage("HttpContext is not available");
    }

    [Fact]
    public void Email_WithUnauthenticatedUser_ThrowsException()
    {
        // Arrange
        var identity = new ClaimsIdentity(); // Not authenticated
        var claimsPrincipal = new ClaimsPrincipal(identity);
        
        var httpContext = new DefaultHttpContext
        {
            User = claimsPrincipal
        };
        
        _httpContextAccessorMock.Setup(x => x.HttpContext).Returns(httpContext);
        var service = new UserContext(_httpContextAccessorMock.Object, _userRepositoryMock.Object);

        // Act
        var act = () => service.Email;

        // Assert
        act.Should().Throw<InvalidOperationException>()
            .WithMessage("User is not authenticated");
    }

    [Fact]
    public void Email_WithMissingEmailClaim_ThrowsException()
    {
        // Arrange
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, "TestUser")
        };
        var identity = new ClaimsIdentity(claims, "TestAuth");
        var claimsPrincipal = new ClaimsPrincipal(identity);
        
        var httpContext = new DefaultHttpContext
        {
            User = claimsPrincipal
        };
        
        _httpContextAccessorMock.Setup(x => x.HttpContext).Returns(httpContext);
        var service = new UserContext(_httpContextAccessorMock.Object, _userRepositoryMock.Object);

        // Act
        var act = () => service.Email;

        // Assert
        act.Should().Throw<InvalidOperationException>()
            .WithMessage("Email claim not found in JWT token");
    }

    [Fact]
    public void UserId_WithNullHttpContext_ThrowsException()
    {
        // Arrange
        _httpContextAccessorMock.Setup(x => x.HttpContext).Returns((HttpContext)null!);
        var service = new UserContext(_httpContextAccessorMock.Object, _userRepositoryMock.Object);

        // Act
        var act = () => service.UserId;

        // Assert
        act.Should().Throw<InvalidOperationException>()
            .WithMessage("HttpContext is not available");
    }
}
