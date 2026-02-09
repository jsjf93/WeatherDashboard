using FluentAssertions;
using WeatherDashboard.Api.Features.Weather.GetForecastByCity;
using WeatherDashboard.Api.Models;

namespace WeatherDashboard.Api.Tests.Mappers;

public class CondensedForecastMapperTests
{
    private readonly CondensedForecastMapper _mapper = new();

    [Fact]
    public void FromEntity_WithValidCondensedForecast_ShouldMapCorrectly()
    {
        // Arrange
        var condensedForecast = new CondensedForecastData
        {
            DailySummaries = new List<DailyForecast>
            {
                new DailyForecast
                {
                    Date = "2024-01-01",
                    Icon = "01d",
                    Temp = 15.5,
                    MinTemp = 10.0,
                    MaxTemp = 20.0,
                    Condition = "Clear",
                    FeelsLike = 14.5,
                    Humidity = 70,
                    Description = "clear sky",
                    WindSpeed = 5.5
                }
            }
        };

        // Act
        var result = _mapper.FromEntity(condensedForecast);

        // Assert
        result.Should().NotBeNull();
        result.DailySummaries.Should().HaveCount(1);
        result.DailySummaries.Should().ContainEquivalentOf(condensedForecast.DailySummaries.First());
    }

    [Fact]
    public void FromEntity_WithMultipleDailySummaries_ShouldMapAllSummaries()
    {
        // Arrange
        var condensedForecast = new CondensedForecastData
        {
            DailySummaries = new List<DailyForecast>
            {
                new DailyForecast
                {
                    Date = "2024-01-01",
                    Icon = "01d",
                    Temp = 15.5,
                    MinTemp = 10.0,
                    MaxTemp = 20.0,
                    Condition = "Clear",
                    FeelsLike = 14.5,
                    Humidity = 70,
                    Description = "clear sky",
                    WindSpeed = 5.5
                },
                new DailyForecast
                {
                    Date = "2024-01-02",
                    Icon = "02d",
                    Temp = 16.5,
                    MinTemp = 11.0,
                    MaxTemp = 21.0,
                    Condition = "Clouds",
                    FeelsLike = 15.5,
                    Humidity = 75,
                    Description = "few clouds",
                    WindSpeed = 6.0
                },
                new DailyForecast
                {
                    Date = "2024-01-03",
                    Icon = "10d",
                    Temp = 14.0,
                    MinTemp = 9.0,
                    MaxTemp = 18.0,
                    Condition = "Rain",
                    FeelsLike = 13.0,
                    Humidity = 85,
                    Description = "light rain",
                    WindSpeed = 7.5
                }
            }
        };

        // Act
        var result = _mapper.FromEntity(condensedForecast);

        // Assert
        result.Should().NotBeNull();
        result.DailySummaries.Should().HaveCount(3);
        result.DailySummaries.Should().BeEquivalentTo(condensedForecast.DailySummaries);
    }

    [Fact]
    public void FromEntity_WithEmptyDailySummaries_ShouldReturnEmptyCollection()
    {
        // Arrange
        var condensedForecast = new CondensedForecastData
        {
            DailySummaries = new List<DailyForecast>()
        };

        // Act
        var result = _mapper.FromEntity(condensedForecast);

        // Assert
        result.Should().NotBeNull();
        result.DailySummaries.Should().BeEmpty();
    }

    [Fact]
    public void FromEntity_PreservesAllDailyForecastProperties()
    {
        // Arrange
        var dailyForecast = new DailyForecast
        {
            Date = "2024-12-25",
            Icon = "13d",
            Temp = -5.5,
            MinTemp = -10.0,
            MaxTemp = -2.0,
            Condition = "Snow",
            FeelsLike = -8.0,
            Humidity = 95,
            Description = "heavy snow",
            WindSpeed = 12.5
        };

        var condensedForecast = new CondensedForecastData
        {
            DailySummaries = new List<DailyForecast> { dailyForecast }
        };

        // Act
        var result = _mapper.FromEntity(condensedForecast);

        // Assert
        var resultForecast = result.DailySummaries.First();
        resultForecast.Date.Should().Be(dailyForecast.Date);
        resultForecast.Icon.Should().Be(dailyForecast.Icon);
        resultForecast.Temp.Should().Be(dailyForecast.Temp);
        resultForecast.MinTemp.Should().Be(dailyForecast.MinTemp);
        resultForecast.MaxTemp.Should().Be(dailyForecast.MaxTemp);
        resultForecast.Condition.Should().Be(dailyForecast.Condition);
        resultForecast.FeelsLike.Should().Be(dailyForecast.FeelsLike);
        resultForecast.Humidity.Should().Be(dailyForecast.Humidity);
        resultForecast.Description.Should().Be(dailyForecast.Description);
        resultForecast.WindSpeed.Should().Be(dailyForecast.WindSpeed);
    }
}
