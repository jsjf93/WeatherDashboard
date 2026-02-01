namespace WeatherDashboard.Api.Features.Favourites.AddFavourite;

public class AddFavouriteRequest
{
    public string City { get; set; } = string.Empty;
}

public class AddFavouriteResponse
{
    public Guid Id { get; set; }
    public string City { get; set; } = string.Empty;
    public bool IsDefault { get; set; }
    public DateTime CreatedAt { get; set; }
}
