using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using System.Text.Json;
using WeatherDashboard.Api.Configuration;
using WeatherDashboard.Api.Models;

namespace WeatherDashboard.Api.Services;

public class WeatherService : IWeatherService
{
    private readonly OpenWeatherMapOptions _options;
    private readonly HttpClient _client;
    private readonly IMemoryCache _cache;

    public WeatherService(IOptions<OpenWeatherMapOptions> options, HttpClient client, IMemoryCache cache)
    {
        _options = options.Value;
        _client = client;
        _cache = cache;
    }

    public async Task<WeatherResponse?> GetWeatherByCityAsync(string city, CancellationToken cancellationToken)
    {
        var cacheKey = $"weather_city_{city.ToLower()}";

        if (_cache.TryGetValue<WeatherResponse>(cacheKey, out var cachedWeather))
        {
            return cachedWeather;
        }

        var url = $"weather?q={city}&appid={_options.ApiKey}&units=metric";

        try
        {
            var response = await _client.GetAsync(url, cancellationToken);

            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return null;
            }

            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync(cancellationToken);
            var openWeatherResponse = JsonSerializer.Deserialize<WeatherResponse>(content, JsonConfiguration.DefaultOptions);

            if (openWeatherResponse == null)
            {
                return null;
            }

            var cacheOptions = new MemoryCacheEntryOptions
            {
                SlidingExpiration = TimeSpan.FromHours(1)
            };

            _cache.Set(cacheKey, openWeatherResponse, cacheOptions);

            return openWeatherResponse;
        }
        catch (HttpRequestException ex)
        {
            throw new Exception("Error fetching weather data from OpenWeatherMap API.", ex);
        }
    }

    public async Task<ForecastResponse?> GetForecastByCityAsync(string city, CancellationToken cancellationToken)
    {
        var cacheKey = $"weather_forecast_city_{city.ToLower()}";

        if (_cache.TryGetValue<ForecastResponse>(cacheKey, out var cachedForecast))
        {
            return cachedForecast;
        }

        var url = $"forecast?q={city}&appid={_options.ApiKey}&units=metric";

        try
        {
            var response = await _client.GetAsync(url, cancellationToken);

            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return null;
            }

            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync(cancellationToken);
            var forecastResponse = JsonSerializer.Deserialize<ForecastResponse>(content, JsonConfiguration.DefaultOptions);

            if (forecastResponse == null)
            {
                return null;
            }

            var cacheOptions = new MemoryCacheEntryOptions
            {
                SlidingExpiration = TimeSpan.FromHours(1)
            };

            _cache.Set(cacheKey, forecastResponse, cacheOptions);

            return forecastResponse;
        }
        catch (HttpRequestException ex)
        {
            throw new Exception("Error fetching forecast data from OpenWeatherMap API.", ex);
        }
    }
}
