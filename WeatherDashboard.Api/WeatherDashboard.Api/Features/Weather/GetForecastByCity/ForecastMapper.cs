using FastEndpoints;
using WeatherDashboard.Api.Models;

namespace WeatherDashboard.Api.Features.Weather.GetForecastByCity;

public sealed class ForecastMapper : Mapper<GetForecastByCityRequest, GetForecastByCityResponse, ForecastResponse>
{
    public override GetForecastByCityResponse FromEntity(ForecastResponse forecast)
    {
        var response = new GetForecastByCityResponse();

        response.FullForecast = forecast.List.Select(item => new ForecastItem
        {
            Dt = item.Dt,
            DtTxt = item.DtTxt,
            Temp = item.Main.Temp,
            FeelsLike = item.Main.FeelsLike,
            TempMin = item.Main.TempMin,
            TempMax = item.Main.TempMax,
            Pressure = item.Main.Pressure,
            Humidity = item.Main.Humidity,
            Condition = item.Weather.FirstOrDefault()?.Main ?? string.Empty,
            Description = item.Weather.FirstOrDefault()?.Description ?? string.Empty,
            Icon = item.Weather.FirstOrDefault()?.Icon ?? string.Empty,
            Clouds = item.Clouds.All,
            WindSpeed = item.Wind.Speed,
            WindDeg = item.Wind.Deg,
            Pop = item.Pop,
            Rain3h = item.Rain?.ThreeH,
            Snow3h = item.Snow?.ThreeH
        }).ToList();

        var groupedByDate = forecast.List
            .GroupBy(item => DateTime.Parse(item.DtTxt).Date)
            .Take(3)
            .ToList();

        response.DailySummaries = groupedByDate.Select(group =>
        {
            var date = group.Key;
            var items = group.ToList();

            // Think I might change this to pick the highest temp of the day
            var middayForecast = items
                .Select(item => new
                {
                    Item = item,
                    Time = DateTime.Parse(item.DtTxt),
                    DistanceToMidday = Math.Abs((DateTime.Parse(item.DtTxt).TimeOfDay - TimeSpan.FromHours(12)).TotalMinutes)
                })
                .OrderBy(x => x.DistanceToMidday)
                .First();

            return new DailyForecast
            {
                Date = date.ToString("yyyy-MM-dd"),
                Icon = middayForecast.Item.Weather.FirstOrDefault()?.Icon ?? string.Empty,
                Temp = middayForecast.Item.Main.Temp,
                MinTemp = items.Min(i => i.Main.TempMin),
                MaxTemp = items.Max(i => i.Main.TempMax),
                Condition = middayForecast.Item.Weather.FirstOrDefault()?.Main ?? string.Empty
            };
        }).ToList();

        return response;
    }
}
