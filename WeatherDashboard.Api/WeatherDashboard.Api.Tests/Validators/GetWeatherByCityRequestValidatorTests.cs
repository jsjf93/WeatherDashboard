using FastEndpoints;
using FluentAssertions;
using WeatherDashboard.Api.Features.Weather.GetWeatherByCity;

namespace WeatherDashboard.Api.Tests.Validators;

public class GetWeatherByCityRequestValidatorTests
{
    private readonly GetWeatherByCityRequestValidator _validator = new();

    [Fact]
    public async Task Validate_WithValidCity_ShouldPass()
    {
        // Arrange
        var request = new GetWeatherByCityRequest("London");

        // Act
        var result = await _validator.ValidateAsync(request);

        // Assert
        result.IsValid.Should().BeTrue();
        result.Errors.Should().BeEmpty();
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    [InlineData(" ")]
    public async Task Validate_WithEmptyCity_ShouldFail(string? city)
    {
        // Arrange
        var request = new GetWeatherByCityRequest(city!);

        // Act
        var result = await _validator.ValidateAsync(request);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.ErrorMessage == "City is required.");
    }

    [Fact]
    public async Task Validate_WithCityTooShort_ShouldFail()
    {
        // Arrange
        var request = new GetWeatherByCityRequest("A");

        // Act
        var result = await _validator.ValidateAsync(request);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle()
            .Which.ErrorMessage.Should().Be("City must be at least 2 characters long.");
    }

    [Fact]
    public async Task Validate_WithCityTooLong_ShouldFail()
    {
        // Arrange
        var city = new string('A', 101);
        var request = new GetWeatherByCityRequest(city);

        // Act
        var result = await _validator.ValidateAsync(request);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle()
            .Which.ErrorMessage.Should().Be("City must be at most 100 characters long.");
    }

    [Theory]
    [InlineData("AB")]
    [InlineData("London")]
    [InlineData("New York")]
    public async Task Validate_WithValidCityLength_ShouldPass(string city)
    {
        // Arrange
        var request = new GetWeatherByCityRequest(city);

        // Act
        var result = await _validator.ValidateAsync(request);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public async Task Validate_WithCityExactly100Characters_ShouldPass()
    {
        // Arrange
        var city = new string('A', 100);
        var request = new GetWeatherByCityRequest(city);

        // Act
        var result = await _validator.ValidateAsync(request);

        // Assert
        result.IsValid.Should().BeTrue();
    }
}
