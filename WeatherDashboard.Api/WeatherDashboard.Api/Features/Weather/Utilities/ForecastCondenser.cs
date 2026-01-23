using WeatherDashboard.Api.Models;

namespace WeatherDashboard.Api.Features.Weather.Utilities;

public static class ForecastCondenser
{
    public static CondensedForecastData Condense(ForecastResponse forecast)
    {
        var groupedByDate = forecast.List
            .GroupBy(item => DateTime.Parse(item.DtTxt).Date)
            .Take(3)
            .ToList();

        var dailyForecasts = groupedByDate.Select(group =>
        {
            var date = group.Key;
            var items = group.ToList();

            var itemWithMaxTemp = items.MaxBy(i => i.Main.TempMax) ?? null;

            return new DailyForecast
            {
                Date = date.ToString("yyyy-MM-dd"),
                Icon = itemWithMaxTemp?.Weather.FirstOrDefault()?.Icon ?? string.Empty,
                Temp = itemWithMaxTemp?.Main.Temp ?? 0,
                MinTemp = items.Min(i => i.Main.TempMin),
                MaxTemp = items.Max(i => i.Main.TempMax),
                Condition = itemWithMaxTemp?.Weather.FirstOrDefault()?.Main ?? string.Empty,
                FeelsLike = itemWithMaxTemp?.Main.FeelsLike ?? 0,
                Humidity = itemWithMaxTemp?.Main.Humidity ?? 0,
                Description = itemWithMaxTemp?.Weather.FirstOrDefault()?.Description ?? string.Empty,
                WindSpeed = itemWithMaxTemp?.Wind.Speed ?? 0,
            };
        }).ToList();

        var response = new CondensedForecastData
        {
            DailySummaries = dailyForecasts,
        };

        return response;
    }
}
