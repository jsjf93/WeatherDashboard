using System.Text.Json.Serialization;

namespace WeatherDashboard.Api.Models;

public class WeatherResponse
{
    public Coordinates Coord { get; set; } = new();
    public List<WeatherData> Weather { get; set; } = new();
    public string Base { get; set; } = string.Empty;
    public MainData Main { get; set; } = new();
    public int Visibility { get; set; }
    public Wind Wind { get; set; } = new();
    public Clouds Clouds { get; set; } = new();
    public long Dt { get; set; }
    public SystemData Sys { get; set; } = new();
    public int Timezone { get; set; }
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int Cod { get; set; }
}

public class Coordinates
{
    public double Lon { get; set; }
    public double Lat { get; set; }
}

public class MainData
{
    public double Temp { get; set; }

    [JsonPropertyName("feels_like")]
    public double FeelsLike { get; set; }

    [JsonPropertyName("temp_min")]
    public double TempMin { get; set; }

    [JsonPropertyName("temp_max")]
    public double TempMax { get; set; }
    public int Pressure { get; set; }
    public int Humidity { get; set; }

    [JsonPropertyName("sea_level")]
    public int SeaLevel { get; set; }

    [JsonPropertyName("grnd_level")]
    public int GrndLevel { get; set; }
}

public class WeatherData
{
    public int Id { get; set; }
    public string Main { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Icon { get; set; } = string.Empty;
}

public class Wind
{
    public double Speed { get; set; }
    public int Deg { get; set; }
}

public class Clouds
{
    public int All { get; set; }
}

public class SystemData
{
    public int Type { get; set; }
    public int Id { get; set; }
    public string Country { get; set; } = string.Empty;
    public long Sunrise { get; set; }
    public long Sunset { get; set; }
}
