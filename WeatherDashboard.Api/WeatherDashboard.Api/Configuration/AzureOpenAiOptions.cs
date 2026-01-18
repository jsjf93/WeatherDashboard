namespace WeatherDashboard.Api.Configuration;

public sealed class AzureOpenAiOptions
{
    public const string SectionName = "AzureOpenAi";
    public string Endpoint { get; set; } = string.Empty;
    public string ApiKey { get; set; } = string.Empty;
    public string ModelName { get; set; } = string.Empty;
    public string DeploymentName { get; set; } = string.Empty;
}
