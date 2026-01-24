using FluentValidation;

namespace WeatherDashboard.Api.Features.Favourites.SetDefaultFavourite;

public class SetDefaultFavouriteRequestValidator : AbstractValidator<SetDefaultFavouriteRequest>
{
    public SetDefaultFavouriteRequestValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Favourite ID is required");
    }
}
