using FastEndpoints;

namespace WeatherDashboard.Api.Features.Weather.GetForecastByCity;

public sealed class GetForecastByCityRequestValidator : Validator<GetForecastByCityRequest>
{
    public GetForecastByCityRequestValidator()
    {
        RuleFor(r => r.City)
            .NotEmpty().WithMessage("City is required.")
            .MinimumLength(2).WithMessage("City must be at least 2 characters long.")
            .MaximumLength(100).WithMessage("City must be at most 100 characters long.");
    }
}
