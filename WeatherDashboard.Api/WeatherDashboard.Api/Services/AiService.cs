using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using System.Text.Json;
using WeatherDashboard.Api.Configuration;

namespace WeatherDashboard.Api.Services;

public class AiService : IAiService
{
    private readonly AzureOpenAiOptions _options;
    private readonly IMemoryCache _cache;
    private readonly HttpClient _httpClient;

    public AiService(IOptions<AzureOpenAiOptions> options, IMemoryCache cache, HttpClient httpClient)
    {
        _options = options.Value;
        _cache = cache;
        _httpClient = httpClient;
    }

    public async Task<string?> GenerateForecastSummaryAsync(string city, object forecastData, CancellationToken cancellationToken)
    {
        var cacheKey = $"forecast_summary_{city.ToLower()}";

        if (_cache.TryGetValue<string>(cacheKey, out var cachedSummary))
        {
            return cachedSummary;
        }

        try
        {
            var forecastJson = JsonSerializer.Serialize(forecastData);
            var prompt = $@"Provide a concise weather forecast summary for {city} based on this data: {forecastJson}

Requirements:
- Maximum 400 characters
- Include key weather conditions and temperature range
- Be informative and helpful for daily planning
- Do not include any additional formatting or explanations";

            var requestBody = new
            {
                messages = new[]
                {
                    new { role = "system", content = "You are a helpful weather forecast assistant. Provide concise, practical weather summaries." },
                    new { role = "user", content = prompt }
                },
                max_completion_tokens = 6000,
                temperature = 1.0,
            };

            var endpoint = _options.Endpoint.TrimEnd('/');
            var url = $"{endpoint}/openai/deployments/{_options.DeploymentName}/chat/completions?api-version=2024-10-01-preview";
            
            var requestJson = JsonSerializer.Serialize(requestBody);
            var request = new HttpRequestMessage(HttpMethod.Post, url)
            {
                Content = new StringContent(requestJson, System.Text.Encoding.UTF8, "application/json")
            };
            request.Headers.Add("api-key", _options.ApiKey);

            // I might look into the SDK instead
            var response = await _httpClient.SendAsync(request, cancellationToken);
            
            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync(cancellationToken);
                throw new HttpRequestException($"Azure OpenAI API returned {response.StatusCode}: {errorContent}", null, response.StatusCode);
            }

            var content = await response.Content.ReadAsStringAsync(cancellationToken);
            
            using var doc = JsonDocument.Parse(content);
            var root = doc.RootElement;
            
            if (!root.TryGetProperty("choices", out var choices) || choices.GetArrayLength() == 0)
            {
                throw new Exception($"Unexpected response structure from Azure OpenAI. Response: {content}");
            }
            
            var message = choices[0].GetProperty("message");
            var summary = message.GetProperty("content").GetString() ?? "";

            if (summary.Length > 400)
            {
                summary = summary.Substring(0, 397) + "...";
            }

            var cacheOptions = new MemoryCacheEntryOptions
            {
                SlidingExpiration = TimeSpan.FromHours(1)
            };

            _cache.Set(cacheKey, summary, cacheOptions);

            return summary;
        }
        catch (Exception ex)
        {
            throw new Exception($"Error generating forecast summary from Azure OpenAI: {ex.Message}", ex);
        }
    }
}
