using FluentAssertions;
using WeatherDashboard.Api.Features.Weather.GetWeatherByCity;
using WeatherDashboard.Api.Models;

namespace WeatherDashboard.Api.Tests.Mappers;

public class WeatherMapperTests
{
    private readonly WeatherMapper _mapper = new();

    [Fact]
    public void FromEntity_WithValidWeatherResponse_ShouldMapCorrectly()
    {
        // Arrange
        var weatherResponse = new WeatherResponse
        {
            Name = "London",
            Main = new MainData
            {
                Temp = 15.5,
                Humidity = 75,
                TempMin = 10.0,
                TempMax = 20.0
            },
            Wind = new Wind
            {
                Speed = 5.5
            },
            Weather = new List<WeatherData>
            {
                new WeatherData
                {
                    Main = "Clouds",
                    Description = "scattered clouds"
                }
            },
            Sys = new SystemData
            {
                Sunrise = 1623045600,
                Sunset = 1623096000
            },
            Timezone = 3600
        };

        // Act
        var result = _mapper.FromEntity(weatherResponse);

        // Assert
        result.Should().NotBeNull();
        result.City.Should().Be("London");
        result.Temperature.Should().Be(15.5);
        result.Humidity.Should().Be(75);
        result.Wind.Should().Be(5.5);
        result.Condition.Should().Be("Clouds");
        result.Description.Should().Be("scattered clouds");
        result.MinTemperature.Should().Be(10.0);
        result.MaxTemperature.Should().Be(20.0);
        result.Sunrise.Should().Be(1623045600);
        result.Sunset.Should().Be(1623096000);
        result.Timezone.Should().Be(3600);
    }

    [Fact]
    public void FromEntity_WithEmptyWeatherList_ShouldUseDefaultValues()
    {
        // Arrange
        var weatherResponse = new WeatherResponse
        {
            Name = "Paris",
            Main = new MainData
            {
                Temp = 20.0,
                Humidity = 60,
                TempMin = 15.0,
                TempMax = 25.0
            },
            Wind = new Wind
            {
                Speed = 3.5
            },
            Weather = new List<WeatherData>(),
            Sys = new SystemData
            {
                Sunrise = 1623045600,
                Sunset = 1623096000
            },
            Timezone = 7200
        };

        // Act
        var result = _mapper.FromEntity(weatherResponse);

        // Assert
        result.Condition.Should().Be("Unknown");
        result.Description.Should().Be("No description");
    }

    [Fact]
    public void FromEntity_WithMultipleWeatherEntries_ShouldUseFirstEntry()
    {
        // Arrange
        var weatherResponse = new WeatherResponse
        {
            Name = "Tokyo",
            Main = new MainData
            {
                Temp = 18.0,
                Humidity = 80,
                TempMin = 15.0,
                TempMax = 25.0
            },
            Wind = new Wind
            {
                Speed = 4.0
            },
            Weather = new List<WeatherData>
            {
                new WeatherData
                {
                    Main = "Rain",
                    Description = "light rain"
                },
                new WeatherData
                {
                    Main = "Clouds",
                    Description = "overcast clouds"
                }
            },
            Sys = new SystemData
            {
                Sunrise = 1623045600,
                Sunset = 1623096000
            },
            Timezone = 32400
        };

        // Act
        var result = _mapper.FromEntity(weatherResponse);

        // Assert
        result.Condition.Should().Be("Rain");
        result.Description.Should().Be("light rain");
    }

    [Fact]
    public void FromEntity_WithZeroValues_ShouldMapZeroValues()
    {
        // Arrange
        var weatherResponse = new WeatherResponse
        {
            Name = "TestCity",
            Main = new MainData
            {
                Temp = 0,
                Humidity = 0,
                TempMin = 0,
                TempMax = 0
            },
            Wind = new Wind
            {
                Speed = 0
            },
            Weather = new List<WeatherData>
            {
                new WeatherData
                {
                    Main = "Clear",
                    Description = "clear sky"
                }
            },
            Sys = new SystemData
            {
                Sunrise = 0,
                Sunset = 0
            },
            Timezone = 0
        };

        // Act
        var result = _mapper.FromEntity(weatherResponse);

        // Assert
        result.Temperature.Should().Be(0);
        result.Wind.Should().Be(0);
        result.Humidity.Should().Be(0);
        result.MinTemperature.Should().Be(0);
        result.MaxTemperature.Should().Be(0);
    }

    [Fact]
    public void FromEntity_WithNegativeTemperature_ShouldMapNegativeValue()
    {
        // Arrange
        var weatherResponse = new WeatherResponse
        {
            Name = "Moscow",
            Main = new MainData
            {
                Temp = -10.5,
                Humidity = 85,
                TempMin = -15.0,
                TempMax = -5.0
            },
            Wind = new Wind
            {
                Speed = 7.2
            },
            Weather = new List<WeatherData>
            {
                new WeatherData
                {
                    Main = "Snow",
                    Description = "heavy snow"
                }
            },
            Sys = new SystemData
            {
                Sunrise = 1623045600,
                Sunset = 1623096000
            },
            Timezone = 10800
        };

        // Act
        var result = _mapper.FromEntity(weatherResponse);

        // Assert
        result.Temperature.Should().Be(-10.5);
        result.MinTemperature.Should().Be(-15.0);
        result.MaxTemperature.Should().Be(-5.0);
    }
}
