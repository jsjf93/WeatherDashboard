using Microsoft.Extensions.Options;
using System.Text.Json;
using WeatherDashboard.Api.Configuration;
using WeatherDashboard.Api.Models;

namespace WeatherDashboard.Api.Services;

public class WeatherService : IWeatherService
{
    private readonly OpenWeatherMapOptions _options;
    private readonly HttpClient _client;

    public WeatherService(IOptions<OpenWeatherMapOptions> options, HttpClient client)
    {
        _options = options.Value;
        _client = client;
    }

    public async Task<WeatherResponse?> GetWeatherByCityAsync(string city, CancellationToken cancellationToken)
    {
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

            return openWeatherResponse;
        }
        catch (HttpRequestException ex)
        {
            throw new Exception("Error fetching weather data from OpenWeatherMap API.", ex);
        }
    }
}
