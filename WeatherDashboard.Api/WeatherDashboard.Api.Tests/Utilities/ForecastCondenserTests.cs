using FluentAssertions;
using WeatherDashboard.Api.Features.Weather.Utilities;
using WeatherDashboard.Api.Models;

namespace WeatherDashboard.Api.Tests.Utilities;

public class ForecastCondenserTests
{
    [Fact]
    public void Condense_WithValidForecastData_ShouldReturnCondensedData()
    {
        // Arrange
        var forecast = CreateSampleForecast();

        // Act
        var result = ForecastCondenser.Condense(forecast);

        // Assert
        result.Should().NotBeNull();
        result.DailySummaries.Should().NotBeEmpty();
    }

    [Fact]
    public void Condense_ShouldReturnMaximumThreeDays()
    {
        // Arrange
        var forecast = CreateForecastWithMultipleDays(5);

        // Act
        var result = ForecastCondenser.Condense(forecast);

        // Assert
        result.DailySummaries.Should().HaveCount(3);
    }

    [Fact]
    public void Condense_ShouldGroupByDate()
    {
        // Arrange
        var forecast = CreateForecastWithSpecificDates();

        // Act
        var result = ForecastCondenser.Condense(forecast);

        // Assert
        result.DailySummaries.Should().OnlyHaveUniqueItems(d => d.Date);
    }

    [Fact]
    public void Condense_ShouldSelectMaxTemperatureForEachDay()
    {
        // Arrange
        var forecast = new ForecastResponse
        {
            List = new List<ForecastListItem>
            {
                CreateForecastItem("2024-01-01 06:00:00", 10, 8, 12),
                CreateForecastItem("2024-01-01 12:00:00", 15, 12, 18),
                CreateForecastItem("2024-01-01 18:00:00", 12, 10, 14),
            }
        };

        // Act
        var result = ForecastCondenser.Condense(forecast);

        // Assert
        result.DailySummaries.Should().HaveCount(1);
        result.DailySummaries[0].MaxTemp.Should().Be(18);
        result.DailySummaries[0].MinTemp.Should().Be(8);
    }

    [Fact]
    public void Condense_ShouldUseIconFromMaxTemperatureItem()
    {
        // Arrange
        var forecast = new ForecastResponse
        {
            List = new List<ForecastListItem>
            {
                CreateForecastItemWithWeather("2024-01-01 06:00:00", 10, 8, 12, "01d", "Clear"),
                CreateForecastItemWithWeather("2024-01-01 12:00:00", 15, 12, 18, "02d", "Sunny"),
                CreateForecastItemWithWeather("2024-01-01 18:00:00", 12, 10, 14, "03d", "Cloudy"),
            }
        };

        // Act
        var result = ForecastCondenser.Condense(forecast);

        // Assert
        result.DailySummaries.Should().HaveCount(1);
        result.DailySummaries[0].Icon.Should().Be("02d");
        result.DailySummaries[0].Condition.Should().Be("Sunny");
    }

    [Fact]
    public void Condense_WithEmptyList_ShouldReturnEmptyDailySummaries()
    {
        // Arrange
        var forecast = new ForecastResponse
        {
            List = new List<ForecastListItem>()
        };

        // Act
        var result = ForecastCondenser.Condense(forecast);

        // Assert
        result.DailySummaries.Should().BeEmpty();
    }

    [Fact]
    public void Condense_ShouldFormatDateAsYyyyMmDd()
    {
        // Arrange
        var forecast = new ForecastResponse
        {
            List = new List<ForecastListItem>
            {
                CreateForecastItem("2024-01-15 12:00:00", 10, 8, 12)
            }
        };

        // Act
        var result = ForecastCondenser.Condense(forecast);

        // Assert
        result.DailySummaries[0].Date.Should().Be("2024-01-15");
    }

    [Fact]
    public void Condense_ShouldIncludeAllWeatherProperties()
    {
        // Arrange
        var forecast = new ForecastResponse
        {
            List = new List<ForecastListItem>
            {
                CreateForecastItemWithAllProperties("2024-01-01 12:00:00")
            }
        };

        // Act
        var result = ForecastCondenser.Condense(forecast);

        // Assert
        var summary = result.DailySummaries[0];
        summary.Date.Should().NotBeEmpty();
        summary.Icon.Should().NotBeEmpty();
        summary.Temp.Should().BeGreaterThan(0);
        summary.MinTemp.Should().BeGreaterThan(0);
        summary.MaxTemp.Should().BeGreaterThan(0);
        summary.Condition.Should().NotBeEmpty();
        summary.FeelsLike.Should().BeGreaterThan(0);
        summary.Humidity.Should().BeGreaterThan(0);
        summary.Description.Should().NotBeEmpty();
        summary.WindSpeed.Should().BeGreaterThan(0);
    }

    private static ForecastResponse CreateSampleForecast()
    {
        return new ForecastResponse
        {
            List = new List<ForecastListItem>
            {
                CreateForecastItem("2024-01-01 12:00:00", 15, 10, 20),
                CreateForecastItem("2024-01-02 12:00:00", 16, 11, 21),
                CreateForecastItem("2024-01-03 12:00:00", 14, 9, 19),
            }
        };
    }

    private static ForecastResponse CreateForecastWithMultipleDays(int days)
    {
        var list = new List<ForecastListItem>();
        for (int i = 0; i < days; i++)
        {
            list.Add(CreateForecastItem($"2024-01-{i + 1:00} 12:00:00", 15, 10, 20));
        }

        return new ForecastResponse { List = list };
    }

    private static ForecastResponse CreateForecastWithSpecificDates()
    {
        return new ForecastResponse
        {
            List = new List<ForecastListItem>
            {
                CreateForecastItem("2024-01-01 06:00:00", 10, 8, 12),
                CreateForecastItem("2024-01-01 12:00:00", 15, 12, 18),
                CreateForecastItem("2024-01-02 06:00:00", 11, 9, 13),
                CreateForecastItem("2024-01-02 12:00:00", 16, 13, 19),
                CreateForecastItem("2024-01-03 06:00:00", 12, 10, 14),
            }
        };
    }

    private static ForecastListItem CreateForecastItem(string dateTime, double temp, double minTemp, double maxTemp)
    {
        return new ForecastListItem
        {
            DtTxt = dateTime,
            Main = new MainData
            {
                Temp = temp,
                TempMin = minTemp,
                TempMax = maxTemp,
                FeelsLike = temp - 1,
                Humidity = 70
            },
            Weather = new List<WeatherData>
            {
                new WeatherData { Main = "Clear", Description = "clear sky", Icon = "01d" }
            },
            Wind = new Wind { Speed = 5.5 }
        };
    }

    private static ForecastListItem CreateForecastItemWithWeather(
        string dateTime, double temp, double minTemp, double maxTemp, string icon, string condition)
    {
        return new ForecastListItem
        {
            DtTxt = dateTime,
            Main = new MainData
            {
                Temp = temp,
                TempMin = minTemp,
                TempMax = maxTemp,
                FeelsLike = temp - 1,
                Humidity = 70
            },
            Weather = new List<WeatherData>
            {
                new WeatherData { Main = condition, Description = condition.ToLower(), Icon = icon }
            },
            Wind = new Wind { Speed = 5.5 }
        };
    }

    private static ForecastListItem CreateForecastItemWithAllProperties(string dateTime)
    {
        return new ForecastListItem
        {
            DtTxt = dateTime,
            Main = new MainData
            {
                Temp = 15,
                TempMin = 10,
                TempMax = 20,
                FeelsLike = 14,
                Humidity = 75
            },
            Weather = new List<WeatherData>
            {
                new WeatherData 
                { 
                    Main = "Clouds", 
                    Description = "scattered clouds", 
                    Icon = "03d" 
                }
            },
            Wind = new Wind { Speed = 6.5 }
        };
    }
}
