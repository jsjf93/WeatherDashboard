using FastEndpoints;
using WeatherDashboard.Api.Data.Repositories;
using WeatherDashboard.Api.Services;

namespace WeatherDashboard.Api.Features.Favourites.GetFavourites;

public class GetFavouritesEndpoint(IUserContext userContext, UserFavouriteRepository favouriteRepository)
    : EndpointWithoutRequest<GetFavouritesResponse>
{
    public override void Configure()
    {
        Get("/favourites");
        Summary(s =>
        {
            s.Summary = "Get user's favourite cities";
            s.Description = "Retrieves all favourite cities for the authenticated user";
            s.Responses[200] = "Successfully retrieved favourites";
            s.Responses[401] = "Unauthorized";
        });
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        try
        {
            var favourites = await favouriteRepository.GetUserFavouritesAsync(userContext.UserId);

            var response = new GetFavouritesResponse(favourites.Select(f => new FavouriteDto(f.Id, f.City, f.CreatedAt)));

            await Send.OkAsync(response, ct);
        }
        catch (InvalidOperationException ex) when (ex.Message.Contains("not authenticated") || ex.Message.Contains("Email claim"))
        {
            await Send.UnauthorizedAsync(ct);
        }
    }
}
