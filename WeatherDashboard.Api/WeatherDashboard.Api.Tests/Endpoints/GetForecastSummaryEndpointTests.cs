using FastEndpoints;
using FluentAssertions;
using Microsoft.Extensions.Caching.Memory;
using Moq;
using WeatherDashboard.Api.Features.Weather.GetForecastSummary;
using WeatherDashboard.Api.Services;

namespace WeatherDashboard.Api.Tests.Endpoints;

public class GetForecastSummaryEndpointTests
{
    [Fact]
    public void Endpoint_CanBeInstantiatedWithRequiredDependencies()
    {
        // Arrange
        var weatherServiceMock = new Mock<IWeatherService>();
        var aiServiceMock = new Mock<IAiService>();
        var cacheMock = new Mock<IMemoryCache>();
        
        // Act
        var endpoint = new GetForecastSummaryEndpoint(
            weatherServiceMock.Object,
            aiServiceMock.Object,
            cacheMock.Object);

        // Assert
        endpoint.Should().NotBeNull();
    }
}
