namespace WeatherDashboard.Api.Features.Favourites.GetFavourites;

public sealed record GetFavouritesResponse(IEnumerable<FavouriteDto> Favourites);

public sealed record FavouriteDto(Guid Id, string City, DateTime CreatedAt);
