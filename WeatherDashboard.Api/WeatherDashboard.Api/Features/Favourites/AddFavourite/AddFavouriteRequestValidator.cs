using FluentValidation;

namespace WeatherDashboard.Api.Features.Favourites.AddFavourite;

public class AddFavouriteRequestValidator : AbstractValidator<AddFavouriteRequest>
{
    public AddFavouriteRequestValidator()
    {
        RuleFor(x => x.City)
            .NotEmpty()
            .WithMessage("City is required")
            .MaximumLength(256)
            .WithMessage("City must not exceed 256 characters");
    }
}
