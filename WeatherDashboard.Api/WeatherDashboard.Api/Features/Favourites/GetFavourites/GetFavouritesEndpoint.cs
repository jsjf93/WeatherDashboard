using FastEndpoints;
using WeatherDashboard.Api.Data.Repositories;
using WeatherDashboard.Api.Services;

namespace WeatherDashboard.Api.Features.Favourites.GetFavourites;

public class GetFavouritesEndpoint(IHttpContextAccessor httpContextAccessor, IUserContext userContext, UserFavouriteRepository favouriteRepository)
    : EndpointWithoutRequest<GetFavouritesResponse>
{
    public override void Configure()
    {
        Get("/favourites");
        AllowAnonymous();
        Summary(s =>
        {
            s.Summary = "Get user's favourite cities";
            s.Description = "Retrieves all favourite cities for the authenticated user. Returns an empty list if user is not authenticated.";
            s.Responses[200] = "Successfully retrieved favourites";
        });
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        // Check if user is authenticated before attempting to retrieve favourites
        var httpContext = httpContextAccessor.HttpContext;
        if (httpContext?.User?.Identity?.IsAuthenticated != true)
        {
            var emptyResponse = new GetFavouritesResponse(Array.Empty<FavouriteDto>());
            await Send.OkAsync(emptyResponse, ct);
            return;
        }

        try
        {
            var favourites = await favouriteRepository.GetUserFavouritesAsync(userContext.UserId);

            var response = new GetFavouritesResponse(favourites.Select(f => new FavouriteDto(f.Id, f.City, f.CreatedAt)));

            await Send.OkAsync(response, ct);
        }
        catch (InvalidOperationException)
        {
            // If there's any issue getting user context, return empty list
            var emptyResponse = new GetFavouritesResponse(Array.Empty<FavouriteDto>());
            await Send.OkAsync(emptyResponse, ct);
        }
    }
}
