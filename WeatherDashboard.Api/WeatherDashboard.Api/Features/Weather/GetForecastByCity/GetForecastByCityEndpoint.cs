using FastEndpoints;
using WeatherDashboard.Api.Services;

namespace WeatherDashboard.Api.Features.Weather.GetForecastByCity;

public sealed class GetForecastByCityEndpoint : Endpoint<GetForecastByCityRequest, GetForecastByCityResponse, CondensedForecastMapper>
{
    private readonly IWeatherService _weatherService;

    public GetForecastByCityEndpoint(IWeatherService weatherService)
    {
        _weatherService = weatherService;
    }

    public override void Configure()
    {
        Get("/forecast/{city}");
        AllowAnonymous();

        Summary(s =>
        {
            s.Summary = "Get 3-day weather forecast by city name";
            s.Description = "Retrieves a 3-day weather forecast with daily summaries and full 3-hour interval data for the specified city.";
            s.ExampleRequest = new GetForecastByCityRequest("London");
            s.Response<GetForecastByCityResponse>(200, "Successful retrieval of forecast data");
            s.Response(404, "City not found");
        });
    }
    
    public override async Task HandleAsync(GetForecastByCityRequest req, CancellationToken ct)
    {
        var condensedForecast = await _weatherService.GetForecastByCityAsync(req.City, ct);

        if (condensedForecast == null)
        {
            AddError(r => r.City, "City not found", "city.not_found");
            await Send.ErrorsAsync(statusCode: 404, cancellation: ct);
            return;
        }

        Response = Map.FromEntity(condensedForecast);

        await Send.OkAsync(Response, ct);
    }
}

