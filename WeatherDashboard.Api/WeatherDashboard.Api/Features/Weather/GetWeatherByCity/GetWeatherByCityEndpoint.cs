using FastEndpoints;
using WeatherDashboard.Api.Services;

namespace WeatherDashboard.Api.Features.Weather.GetWeatherByCity;

public sealed class GetWeatherByCityEndpoint : Endpoint<GetWeatherByCityRequest, GetWeatherByCityResponse, WeatherMapper> 
{
    private readonly IWeatherService _weatherService;

    public GetWeatherByCityEndpoint(IWeatherService weatherService)
    {
        _weatherService = weatherService;
    }

    public override void Configure()
    {
        Get("/weather/{city}");
        AllowAnonymous();

        Summary(s =>
        {
            s.Summary = "Get weather information by city name";
            s.Description = "Retrieves current weather data for the specified city.";
            s.ExampleRequest = new GetWeatherByCityRequest("London");
            s.Response<GetWeatherByCityResponse>(200, "Successful retrieval of weather data");
            s.Response(404, "City not found");
        });
    }
    
    public override async Task HandleAsync(GetWeatherByCityRequest req, CancellationToken ct)
    {
        var weatherResponse = await _weatherService.GetWeatherByCityAsync(req.City, ct);

        if (weatherResponse == null)
        {
            AddError(r => r.City, "City not found", "city.not_found");
            await Send.ErrorsAsync(statusCode: 404, cancellation: ct);
            return;
        }

        Response = Map.FromEntity(weatherResponse);

        await Send.OkAsync(Response, ct);
    }
}
