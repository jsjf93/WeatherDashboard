namespace WeatherDashboard.Api.Features.Favourites.AddFavourite;

public sealed record AddFavouriteRequest(string City);

public sealed record AddFavouriteResponse(Guid Id, string City, DateTime CreatedAt);
