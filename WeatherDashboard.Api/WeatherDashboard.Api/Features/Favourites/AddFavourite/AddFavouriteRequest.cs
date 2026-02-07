namespace WeatherDashboard.Api.Features.Favourites.AddFavourite;

public record AddFavouriteRequest(string City);

public record AddFavouriteResponse(Guid Id, string City, DateTime CreatedAt);
