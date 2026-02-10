using FastEndpoints;
using WeatherDashboard.Api.Data.Repositories;
using WeatherDashboard.Api.Services;

namespace WeatherDashboard.Api.Features.Favourites.GetFavourites;

public class GetFavouritesEndpoint(IHttpContextAccessor httpContextAccessor, IUserContext userContext, UserFavouriteRepository favouriteRepository)
    : EndpointWithoutRequest<GetFavouritesResponse>
{
    private static readonly GetFavouritesResponse EmptyResponse = new(Array.Empty<FavouriteDto>());

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
            await Send.OkAsync(EmptyResponse, ct);
            return;
        }

        try
        {
            var favourites = await favouriteRepository.GetUserFavouritesAsync(userContext.UserId);

            var response = new GetFavouritesResponse(favourites.Select(f => new FavouriteDto(f.Id, f.City, f.CreatedAt)));

            await Send.OkAsync(response, ct);
        }
        catch (InvalidOperationException ex)
        {
            // Log the exception and return empty list for any user context issues
            // This typically happens when there's a problem with claims or user initialization
            Logger.LogWarning(ex, "Failed to retrieve user context or favourites. Returning empty list.");
            await Send.OkAsync(EmptyResponse, ct);
        }
    }
}
