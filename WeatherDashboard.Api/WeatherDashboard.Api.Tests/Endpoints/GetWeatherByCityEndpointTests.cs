using FastEndpoints;
using FluentAssertions;
using Moq;
using WeatherDashboard.Api.Features.Weather.GetWeatherByCity;
using WeatherDashboard.Api.Services;

namespace WeatherDashboard.Api.Tests.Endpoints;

public class GetWeatherByCityEndpointTests
{
    [Fact]
    public void Endpoint_CanBeInstantiatedWithRequiredDependencies()
    {
        // Arrange
        var weatherServiceMock = new Mock<IWeatherService>();

        // Act
        var endpoint = new GetWeatherByCityEndpoint(weatherServiceMock.Object);

        // Assert
        endpoint.Should().NotBeNull();
    }
}
