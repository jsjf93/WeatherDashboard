global using FluentValidation;

using FastEndpoints;
using FastEndpoints.Swagger;
using Microsoft.Extensions.Options;
using WeatherDashboard.Api.Configuration;
using WeatherDashboard.Api.Services;

var builder = WebApplication.CreateBuilder(args);

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

builder.Services.Configure<OpenWeatherMapOptions>(builder.Configuration.GetSection(OpenWeatherMapOptions.SectionName));

builder.Services.AddHttpClient<IWeatherService, WeatherService>((sp, client) =>
{
    var options = sp.GetRequiredService<IOptions<OpenWeatherMapOptions>>().Value;
    client.BaseAddress = new Uri(options.BaseUrl);
});

var app = builder.Build();

app.UseFastEndpoints(config =>
{
    config.Endpoints.RoutePrefix = "api";
});

app.UseSwaggerGen();

app.UseHttpsRedirection();

app.Run();
