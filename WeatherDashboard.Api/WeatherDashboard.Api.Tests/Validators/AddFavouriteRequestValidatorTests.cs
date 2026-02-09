using FluentAssertions;
using FluentValidation.TestHelper;
using WeatherDashboard.Api.Features.Favourites.AddFavourite;

namespace WeatherDashboard.Api.Tests.Validators;

public class AddFavouriteRequestValidatorTests
{
    private readonly AddFavouriteRequestValidator _validator = new();

    [Fact]
    public async Task Validate_WithValidCity_ShouldPass()
    {
        // Arrange
        var request = new AddFavouriteRequest("Tokyo");

        // Act
        var result = await _validator.TestValidateAsync(request);

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
        var request = new AddFavouriteRequest(city!);

        // Act
        var result = await _validator.TestValidateAsync(request);

        // Assert
        result.IsValid.Should().BeFalse();
        result.ShouldHaveValidationErrorFor(x => x.City)
            .WithErrorMessage("City is required");
    }

    [Fact]
    public async Task Validate_WithCityExactly256Characters_ShouldPass()
    {
        // Arrange
        var city = new string('A', 256);
        var request = new AddFavouriteRequest(city);

        // Act
        var result = await _validator.TestValidateAsync(request);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public async Task Validate_WithCityTooLong_ShouldFail()
    {
        // Arrange
        var city = new string('B', 257);
        var request = new AddFavouriteRequest(city);

        // Act
        var result = await _validator.TestValidateAsync(request);

        // Assert
        result.IsValid.Should().BeFalse();
        result.ShouldHaveValidationErrorFor(x => x.City)
            .WithErrorMessage("City must not exceed 256 characters");
    }

    [Theory]
    [InlineData("A")]
    [InlineData("London")]
    [InlineData("San Francisco")]
    public async Task Validate_WithValidCityNames_ShouldPass(string city)
    {
        // Arrange
        var request = new AddFavouriteRequest(city);

        // Act
        var result = await _validator.TestValidateAsync(request);

        // Assert
        result.IsValid.Should().BeTrue();
    }
}
