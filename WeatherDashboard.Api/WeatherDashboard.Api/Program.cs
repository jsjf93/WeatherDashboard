global using FluentValidation;

using FastEndpoints;
using FastEndpoints.Swagger;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.Identity.Web;
using WeatherDashboard.Api.Configuration;
using WeatherDashboard.Api.Services;

var builder = WebApplication.CreateBuilder(args);

var frontendUrl = builder.Configuration["FrontendUrl"] ?? throw new ArgumentNullException("FrontendUrl is not set in configuration");
var tenantId = builder.Configuration["AzureAd:TenantId"] ?? throw new ArgumentNullException("AzureAd:TenantId is not set in configuration");
var clientId = builder.Configuration["AzureAd:ClientId"] ?? throw new ArgumentNullException("AzureAd:ClientId is not set in configuration");

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddMicrosoftIdentityWebApi(options =>
    {
        builder.Configuration.Bind("AzureAd", options);
        options.Authority = $"https://login.microsoftonline.com/{tenantId}/v2.0";
        options.Audience = clientId;
    }, options =>
    {
        builder.Configuration.Bind("AzureAd", options);
    });

builder.Services.AddAuthorization();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins(frontendUrl)
              .AllowAnyMethod()
              .AllowAnyHeader()
              .AllowCredentials();
    });
});

builder.Services
    .AddFastEndpoints()
    .SwaggerDocument(options =>
    {
        options.DocumentSettings = doc =>
        {
            doc.Title = "Weather Dashboard API";
            doc.Version = "v1";
            doc.Description = "API for Weather Dashboard application.";
        };
    });

builder.Services.AddMemoryCache();

builder.Services.Configure<OpenWeatherMapOptions>(builder.Configuration.GetSection(OpenWeatherMapOptions.SectionName));

builder.Services.AddHttpClient<IWeatherService, WeatherService>((sp, client) =>
{
    var options = sp.GetRequiredService<IOptions<OpenWeatherMapOptions>>().Value;
    client.BaseAddress = new Uri(options.BaseUrl);
});

builder.Services.Configure<AzureOpenAiOptions>(builder.Configuration.GetSection(AzureOpenAiOptions.SectionName));
builder.Services.AddHttpClient<IAiService, AiService>();

var app = builder.Build();

app.UseCors();

app.UseAuthentication();
app.UseAuthorization();

app.UseFastEndpoints(config =>
{
    config.Endpoints.RoutePrefix = "api";
});

app.UseSwaggerGen();

app.UseHttpsRedirection();

app.Run();
