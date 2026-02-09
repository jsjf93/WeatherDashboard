using FastEndpoints;
using FluentAssertions;
using Moq;
using WeatherDashboard.Api.Data.Repositories;
using WeatherDashboard.Api.Features.Favourites.GetFavourites;
using WeatherDashboard.Api.Services;

namespace WeatherDashboard.Api.Tests.Endpoints;

public class GetFavouritesEndpointTests
{
    [Fact]
    public void Endpoint_CanBeInstantiatedWithRequiredDependencies()
    {
        // Arrange
        var userContextMock = new Mock<IUserContext>();
        var repositoryMock = new Mock<UserFavouriteRepository>(MockBehavior.Strict, null!);
        
        // Act
        var endpoint = new GetFavouritesEndpoint(
            userContextMock.Object,
            repositoryMock.Object);

        // Assert
        endpoint.Should().NotBeNull();
    }
}
