using FastEndpoints;
using WeatherDashboard.Api.Data.Repositories;
using WeatherDashboard.Api.Services;

namespace WeatherDashboard.Api.Features.Favourites.RemoveFavourite;

public class RemoveFavouriteEndpoint(IUserContext userContext, UserFavouriteRepository favouriteRepository)
    : Endpoint<RemoveFavouriteRequest>
{
    public override void Configure()
    {
        Delete("/favourites/{id}");
        Summary(s =>
        {
            s.Summary = "Remove a favourite city";
            s.Description = "Removes a favourite city from the authenticated user's list";
            s.Responses[200] = "Successfully removed favourite";
            s.Responses[404] = "Favourite not found";
            s.Responses[401] = "Unauthorized";
        });
    }

    public override async Task HandleAsync(RemoveFavouriteRequest req, CancellationToken ct)
    {
        try
        {
            var favourite = await favouriteRepository.GetFavouriteByIdAsync(req.Id);

            if (favourite == null || favourite.UserId != userContext.UserId)
            {
                ThrowError("Favourite not found", 404);
            }

            await favouriteRepository.RemoveFavouriteAsync(req.Id);

            await Send.NoContentAsync(ct);
        }
        catch (InvalidOperationException ex) when (ex.Message.Contains("not authenticated") || ex.Message.Contains("Email claim"))
        {
            await Send.UnauthorizedAsync(ct);
        }
    }
}
