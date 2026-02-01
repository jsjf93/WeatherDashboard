namespace WeatherDashboard.Api.Features.Favourites.GetFavourites;

public class GetFavouritesResponse
{
    public List<FavouriteDto> Favourites { get; set; } = [];
}

public class FavouriteDto
{
    public Guid Id { get; set; }
    public string City { get; set; } = string.Empty;
    public bool IsDefault { get; set; }
    public DateTime CreatedAt { get; set; }
}
