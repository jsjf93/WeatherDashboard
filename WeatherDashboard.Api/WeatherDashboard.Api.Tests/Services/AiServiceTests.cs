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

public class AiServiceTests
{
    private readonly Mock<IOptions<AzureOpenAiOptions>> _optionsMock;
    private readonly Mock<HttpMessageHandler> _httpMessageHandlerMock;
    private readonly HttpClient _httpClient;
    private readonly IMemoryCache _cache;
    private readonly AiService _service;

    public AiServiceTests()
    {
        _optionsMock = new Mock<IOptions<AzureOpenAiOptions>>();
        _optionsMock.Setup(x => x.Value).Returns(new AzureOpenAiOptions
        {
            Endpoint = "https://test.openai.azure.com",
            ApiKey = "test-api-key",
            DeploymentName = "test-deployment"
        });

        _httpMessageHandlerMock = new Mock<HttpMessageHandler>();
        _httpClient = new HttpClient(_httpMessageHandlerMock.Object);

        _cache = new MemoryCache(new MemoryCacheOptions());
        _service = new AiService(_optionsMock.Object, _cache, _httpClient);
    }

    [Fact]
    public async Task GenerateForecastSummaryAsync_WithValidForecast_ReturnsSummary()
    {
        // Arrange
        var city = "London";
        var forecastData = new CondensedForecastData
        {
            DailySummaries = new List<DailyForecast>
            {
                new DailyForecast
                {
                    Date = "2024-01-01",
                    Temp = 15,
                    MinTemp = 10,
                    MaxTemp = 20,
                    Condition = "Clear"
                }
            }
        };

        var aiResponse = new
        {
            choices = new[]
            {
                new
                {
                    message = new
                    {
                        content = "Weather forecast for London: Clear skies with temperatures between 10-20°C."
                    }
                }
            }
        };

        var jsonResponse = JsonSerializer.Serialize(aiResponse);
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
        var result = await _service.GenerateForecastSummaryAsync(city, forecastData, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Should().Contain("London");
    }

    [Fact]
    public async Task GenerateForecastSummaryAsync_CachesResults()
    {
        // Arrange
        var city = "Paris";
        var forecastData = new CondensedForecastData
        {
            DailySummaries = new List<DailyForecast>
            {
                new DailyForecast { Date = "2024-01-01", Temp = 18 }
            }
        };

        var aiResponse = new
        {
            choices = new[]
            {
                new
                {
                    message = new
                    {
                        content = "Test forecast summary"
                    }
                }
            }
        };

        var jsonResponse = JsonSerializer.Serialize(aiResponse);
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
        var result1 = await _service.GenerateForecastSummaryAsync(city, forecastData, CancellationToken.None);
        var result2 = await _service.GenerateForecastSummaryAsync(city, forecastData, CancellationToken.None);

        // Assert
        result1.Should().NotBeNull();
        result2.Should().NotBeNull();
        result1.Should().Be(result2);
        
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
    public async Task GenerateForecastSummaryAsync_WithLongResponse_TruncatesTo400Characters()
    {
        // Arrange
        var city = "Tokyo";
        var forecastData = new CondensedForecastData
        {
            DailySummaries = new List<DailyForecast>
            {
                new DailyForecast { Date = "2024-01-01", Temp = 20 }
            }
        };

        var longContent = new string('A', 500);
        var aiResponse = new
        {
            choices = new[]
            {
                new
                {
                    message = new
                    {
                        content = longContent
                    }
                }
            }
        };

        var jsonResponse = JsonSerializer.Serialize(aiResponse);
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
        var result = await _service.GenerateForecastSummaryAsync(city, forecastData, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result!.Length.Should().Be(400);
        result.Should().EndWith("...");
    }

    [Fact]
    public async Task GenerateForecastSummaryAsync_WithHttpError_ThrowsException()
    {
        // Arrange
        var city = "TestCity";
        var forecastData = new CondensedForecastData { DailySummaries = new List<DailyForecast>() };
        
        var httpResponse = new HttpResponseMessage(HttpStatusCode.InternalServerError)
        {
            Content = new StringContent("Server error")
        };

        _httpMessageHandlerMock
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(httpResponse);

        // Act & Assert
        var act = async () => await _service.GenerateForecastSummaryAsync(city, forecastData, CancellationToken.None);
        await act.Should().ThrowAsync<Exception>()
            .WithMessage("*Azure OpenAI*");
    }

    [Fact]
    public async Task GenerateForecastSummaryAsync_WithMissingChoices_ThrowsException()
    {
        // Arrange
        var city = "TestCity";
        var forecastData = new CondensedForecastData { DailySummaries = new List<DailyForecast>() };
        
        var aiResponse = new
        {
            choices = Array.Empty<object>()
        };

        var jsonResponse = JsonSerializer.Serialize(aiResponse);
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

        // Act & Assert
        var act = async () => await _service.GenerateForecastSummaryAsync(city, forecastData, CancellationToken.None);
        await act.Should().ThrowAsync<Exception>()
            .WithMessage("*Unexpected response structure*");
    }

    [Fact]
    public async Task GenerateForecastSummaryAsync_CaseInsensitiveCity_UsesSameCacheKey()
    {
        // Arrange
        var forecastData = new CondensedForecastData
        {
            DailySummaries = new List<DailyForecast>
            {
                new DailyForecast { Date = "2024-01-01", Temp = 15 }
            }
        };

        var aiResponse = new
        {
            choices = new[]
            {
                new
                {
                    message = new
                    {
                        content = "Test summary"
                    }
                }
            }
        };

        var jsonResponse = JsonSerializer.Serialize(aiResponse);
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
        var result1 = await _service.GenerateForecastSummaryAsync("London", forecastData, CancellationToken.None);
        var result2 = await _service.GenerateForecastSummaryAsync("LONDON", forecastData, CancellationToken.None);

        // Assert
        result1.Should().Be(result2);
        
        // Verify HTTP was only called once (cached)
        _httpMessageHandlerMock
            .Protected()
            .Verify(
                "SendAsync",
                Times.Once(),
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>());
    }
}
