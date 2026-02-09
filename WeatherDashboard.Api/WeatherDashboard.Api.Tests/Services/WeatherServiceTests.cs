using FluentAssertions;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Moq;
using Moq.Protected;
using System.Net;
using System.Text.Json;
using WeatherDashboard.Api.Configuration;
using WeatherDashboard.Api.Models;
using WeatherDashboard.Api.Services;

namespace WeatherDashboard.Api.Tests.Services;

public class WeatherServiceTests
{
    private readonly Mock<IOptions<OpenWeatherMapOptions>> _optionsMock;
    private readonly Mock<HttpMessageHandler> _httpMessageHandlerMock;
    private readonly HttpClient _httpClient;
    private readonly IMemoryCache _cache;
    private readonly WeatherService _service;

    public WeatherServiceTests()
    {
        _optionsMock = new Mock<IOptions<OpenWeatherMapOptions>>();
        _optionsMock.Setup(x => x.Value).Returns(new OpenWeatherMapOptions
        {
            ApiKey = "test-api-key",
            BaseUrl = "https://api.openweathermap.org/data/2.5/"
        });

        _httpMessageHandlerMock = new Mock<HttpMessageHandler>();
        _httpClient = new HttpClient(_httpMessageHandlerMock.Object)
        {
            BaseAddress = new Uri("https://api.openweathermap.org/data/2.5/")
        };

        _cache = new MemoryCache(new MemoryCacheOptions());
        _service = new WeatherService(_optionsMock.Object, _httpClient, _cache);
    }

    [Fact]
    public async Task GetWeatherByCityAsync_WithValidCity_ReturnsWeatherData()
    {
        // Arrange
        var expectedWeather = new WeatherResponse
        {
            Name = "London",
            Main = new MainData { Temp = 15.5, Humidity = 70 },
            Wind = new Wind { Speed = 5.5 },
            Weather = new List<WeatherData>
            {
                new WeatherData { Main = "Clear", Description = "clear sky" }
            }
        };

        var jsonResponse = JsonSerializer.Serialize(expectedWeather);
        var httpResponse = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent(jsonResponse)
        };

        _httpMessageHandlerMock
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(httpResponse);

        // Act
        var result = await _service.GetWeatherByCityAsync("London", CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result!.Name.Should().Be("London");
        result.Main.Temp.Should().Be(15.5);
        result.Wind.Speed.Should().Be(5.5);
    }

    [Fact]
    public async Task GetWeatherByCityAsync_WithNonExistentCity_ReturnsNull()
    {
        // Arrange
        var httpResponse = new HttpResponseMessage(HttpStatusCode.NotFound);

        _httpMessageHandlerMock
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(httpResponse);

        // Act
        var result = await _service.GetWeatherByCityAsync("NonExistentCity", CancellationToken.None);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task GetWeatherByCityAsync_CachesResults()
    {
        // Arrange
        var expectedWeather = new WeatherResponse
        {
            Name = "Paris",
            Main = new MainData { Temp = 20.0 },
            Wind = new Wind { Speed = 3.5 },
            Weather = new List<WeatherData> { new WeatherData { Main = "Clouds" } }
        };

        var jsonResponse = JsonSerializer.Serialize(expectedWeather);
        var httpResponse = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent(jsonResponse)
        };

        _httpMessageHandlerMock
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(httpResponse);

        // Act - First call
        var result1 = await _service.GetWeatherByCityAsync("Paris", CancellationToken.None);
        
        // Act - Second call (should be from cache)
        var result2 = await _service.GetWeatherByCityAsync("Paris", CancellationToken.None);

        // Assert
        result1.Should().NotBeNull();
        result2.Should().NotBeNull();
        result1.Should().BeEquivalentTo(result2);
        
        // Verify HTTP was only called once
        _httpMessageHandlerMock
            .Protected()
            .Verify(
                "SendAsync",
                Times.Once(),
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>());
    }

    [Fact]
    public async Task GetWeatherByCityAsync_WithHttpError_ThrowsException()
    {
        // Arrange
        var httpResponse = new HttpResponseMessage(HttpStatusCode.InternalServerError);

        _httpMessageHandlerMock
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(httpResponse);

        // Act & Assert
        var act = async () => await _service.GetWeatherByCityAsync("TestCity", CancellationToken.None);
        await act.Should().ThrowAsync<Exception>()
            .WithMessage("Error fetching weather data from OpenWeatherMap API.");
    }

    [Fact]
    public async Task GetForecastByCityAsync_WithValidCity_ReturnsForecastData()
    {
        // Arrange
        var forecastResponse = new ForecastResponse
        {
            List = new List<ForecastListItem>
            {
                new ForecastListItem
                {
                    DtTxt = "2024-01-01 12:00:00",
                    Main = new MainData { Temp = 15, TempMin = 10, TempMax = 20, FeelsLike = 14, Humidity = 70 },
                    Weather = new List<WeatherData> { new WeatherData { Main = "Clear", Icon = "01d", Description = "clear sky" } },
                    Wind = new Wind { Speed = 5.5 }
                },
                new ForecastListItem
                {
                    DtTxt = "2024-01-02 12:00:00",
                    Main = new MainData { Temp = 16, TempMin = 11, TempMax = 21, FeelsLike = 15, Humidity = 75 },
                    Weather = new List<WeatherData> { new WeatherData { Main = "Clouds", Icon = "02d", Description = "few clouds" } },
                    Wind = new Wind { Speed = 6.0 }
                }
            }
        };

        var jsonResponse = JsonSerializer.Serialize(forecastResponse);
        var httpResponse = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent(jsonResponse)
        };

        _httpMessageHandlerMock
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(httpResponse);

        // Act
        var result = await _service.GetForecastByCityAsync("London", CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result!.DailySummaries.Should().NotBeEmpty();
    }

    [Fact]
    public async Task GetForecastByCityAsync_WithNonExistentCity_ReturnsNull()
    {
        // Arrange
        var httpResponse = new HttpResponseMessage(HttpStatusCode.NotFound);

        _httpMessageHandlerMock
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(httpResponse);

        // Act
        var result = await _service.GetForecastByCityAsync("NonExistentCity", CancellationToken.None);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task GetForecastByCityAsync_CachesResults()
    {
        // Arrange
        var forecastResponse = new ForecastResponse
        {
            List = new List<ForecastListItem>
            {
                new ForecastListItem
                {
                    DtTxt = "2024-01-01 12:00:00",
                    Main = new MainData { Temp = 15, TempMin = 10, TempMax = 20, FeelsLike = 14, Humidity = 70 },
                    Weather = new List<WeatherData> { new WeatherData { Main = "Clear", Icon = "01d", Description = "clear" } },
                    Wind = new Wind { Speed = 5.5 }
                }
            }
        };

        var jsonResponse = JsonSerializer.Serialize(forecastResponse);
        var httpResponse = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent(jsonResponse)
        };

        _httpMessageHandlerMock
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(httpResponse);

        // Act
        var result1 = await _service.GetForecastByCityAsync("Tokyo", CancellationToken.None);
        var result2 = await _service.GetForecastByCityAsync("Tokyo", CancellationToken.None);

        // Assert
        result1.Should().NotBeNull();
        result2.Should().NotBeNull();
        result1.Should().BeEquivalentTo(result2);
        
        _httpMessageHandlerMock
            .Protected()
            .Verify(
                "SendAsync",
                Times.Once(),
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>());
    }
}
