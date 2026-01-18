using System.Text.Json;

namespace WeatherDashboard.Api.Configuration;

public static class JsonConfiguration
{
    public static JsonSerializerOptions DefaultOptions { get; } = new()
    {
        PropertyNameCaseInsensitive = true
    };
}
