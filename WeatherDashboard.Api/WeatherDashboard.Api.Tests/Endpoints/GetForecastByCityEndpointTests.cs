using FastEndpoints;
using FluentAssertions;
using Moq;
using WeatherDashboard.Api.Features.Weather.GetForecastByCity;
using WeatherDashboard.Api.Services;

namespace WeatherDashboard.Api.Tests.Endpoints;

public class GetForecastByCityEndpointTests
{
    [Fact]
    public void Endpoint_CanBeInstantiatedWithRequiredDependencies()
    {
        // Arrange
        var weatherServiceMock = new Mock<IWeatherService>();

        // Act
        var endpoint = new GetForecastByCityEndpoint(weatherServiceMock.Object);

        // Assert
        endpoint.Should().NotBeNull();
    }
}
