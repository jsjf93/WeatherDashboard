using FastEndpoints;
using Microsoft.Extensions.Caching.Memory;
using WeatherDashboard.Api.Models;
using WeatherDashboard.Api.Services;

namespace WeatherDashboard.Api.Features.Weather.GetForecastSummary;

public sealed class GetForecastSummaryEndpoint : Endpoint<GetForecastSummaryRequest, GetForecastSummaryResponse>
{
    private readonly IWeatherService _weatherService;
    private readonly IAiService _aiService;
    private readonly IMemoryCache _cache;

    public GetForecastSummaryEndpoint(IWeatherService weatherService, IAiService aiService, IMemoryCache cache)
    {
        _weatherService = weatherService;
        _aiService = aiService;
        _cache = cache;
    }

    public override void Configure()
    {
        Get("/forecast/{city}/summary");
        AllowAnonymous();

        Summary(s =>
        {
            s.Summary = "Get AI-generated weather forecast summary";
            s.Description = "Retrieves an AI-generated summary of the weather forecast (max 400 characters) for the specified city.";
            s.ExampleRequest = new GetForecastSummaryRequest("London");
            s.Response<GetForecastSummaryResponse>(200, "Successful retrieval of forecast summary");
            s.Response(404, "Forecast data not found for the specified city");
        });
    }

    public override async Task HandleAsync(GetForecastSummaryRequest req, CancellationToken ct)
    {
        var cacheKey = $"weather_forecast_city_{req.City.ToLower()}";
        
        if (!_cache.TryGetValue<CondensedForecastData>(cacheKey, out var forecastData) || forecastData == null)
        {
            AddError(r => r.City, "Forecast not found. Please request the forecast endpoint first.", "forecast.not_found");
            await Send.ErrorsAsync(statusCode: 404, cancellation: ct);
            return;
        }

        var summary = await _aiService.GenerateForecastSummaryAsync(req.City, forecastData, ct);

        if (summary == null)
        {
            AddError(r => r.City, "Failed to generate forecast summary.", "summary.generation_failed");
            await Send.ErrorsAsync(statusCode: 500, cancellation: ct);
            return;
        }

        Response = new GetForecastSummaryResponse(summary);

        await Send.OkAsync(Response, ct);
    }
}
