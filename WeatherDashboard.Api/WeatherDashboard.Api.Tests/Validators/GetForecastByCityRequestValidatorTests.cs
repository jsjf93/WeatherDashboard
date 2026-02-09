using FastEndpoints;
using FluentAssertions;
using WeatherDashboard.Api.Features.Weather.GetForecastByCity;

namespace WeatherDashboard.Api.Tests.Validators;

public class GetForecastByCityRequestValidatorTests
{
    private readonly GetForecastByCityRequestValidator _validator = new();

    [Fact]
    public async Task Validate_WithValidCity_ShouldPass()
    {
        // Arrange
        var request = new GetForecastByCityRequest("London");

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
        var request = new GetForecastByCityRequest(city!);

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
        var request = new GetForecastByCityRequest("X");

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
        var city = new string('B', 101);
        var request = new GetForecastByCityRequest(city);

        // Act
        var result = await _validator.ValidateAsync(request);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle()
            .Which.ErrorMessage.Should().Be("City must be at most 100 characters long.");
    }

    [Theory]
    [InlineData("NY")]
    [InlineData("Paris")]
    [InlineData("Los Angeles")]
    public async Task Validate_WithValidCityLength_ShouldPass(string city)
    {
        // Arrange
        var request = new GetForecastByCityRequest(city);

        // Act
        var result = await _validator.ValidateAsync(request);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public async Task Validate_WithCityExactly100Characters_ShouldPass()
    {
        // Arrange
        var city = new string('C', 100);
        var request = new GetForecastByCityRequest(city);

        // Act
        var result = await _validator.ValidateAsync(request);

        // Assert
        result.IsValid.Should().BeTrue();
    }
}
