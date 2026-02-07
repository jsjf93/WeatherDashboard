namespace WeatherDashboard.Api.Features.Favourites.GetFavourites;

public record GetFavouritesResponse(IEnumerable<FavouriteDto> Favourites);

public record FavouriteDto(Guid Id, string City, DateTime CreatedAt);
