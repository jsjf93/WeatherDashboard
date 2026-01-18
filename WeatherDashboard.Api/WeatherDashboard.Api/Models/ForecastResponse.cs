using System.Text.Json.Serialization;

namespace WeatherDashboard.Api.Models;

public class ForecastResponse
{
    public string Cod { get; set; } = string.Empty;
    public int Message { get; set; }
    public int Cnt { get; set; }
    public List<ForecastListItem> List { get; set; } = new();
    public CityInfo City { get; set; } = new();
}

public class ForecastListItem
{
    public long Dt { get; set; }
    public MainData Main { get; set; } = new();
    public List<WeatherData> Weather { get; set; } = new();
    public Clouds Clouds { get; set; } = new();
    public Wind Wind { get; set; } = new();
    public int Visibility { get; set; }
    public double Pop { get; set; }
    public Rain? Rain { get; set; }
    public Snow? Snow { get; set; }
    public Sys Sys { get; set; } = new();

    [JsonPropertyName("dt_txt")]
    public string DtTxt { get; set; } = string.Empty;
}

public class Rain
{
    [JsonPropertyName("3h")]
    public double ThreeH { get; set; }
}

public class Snow
{
    [JsonPropertyName("3h")]
    public double ThreeH { get; set; }
}

public class Sys
{
    public string Pod { get; set; } = string.Empty;
}

public class CityInfo
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public Coordinates Coord { get; set; } = new();
    public string Country { get; set; } = string.Empty;
    public int Population { get; set; }
    public int Timezone { get; set; }
    public long Sunrise { get; set; }
    public long Sunset { get; set; }
}
