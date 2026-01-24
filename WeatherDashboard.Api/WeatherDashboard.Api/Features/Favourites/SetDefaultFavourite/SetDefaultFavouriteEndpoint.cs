using FastEndpoints;
using WeatherDashboard.Api.Data.Repositories;
using WeatherDashboard.Api.Services;

namespace WeatherDashboard.Api.Features.Favourites.SetDefaultFavourite;

public class SetDefaultFavouriteEndpoint(IUserContext userContext, UserFavouriteRepository favouriteRepository)
    : Endpoint<SetDefaultFavouriteRequest>
{
    public override void Configure()
    {
        Put("/favourites/{id}/set-default");
        Summary(s =>
        {
            s.Summary = "Set a favourite as default";
            s.Description = "Sets a favourite city as the default for the authenticated user";
            s.Responses[200] = "Successfully set as default";
            s.Responses[404] = "Favourite not found";
            s.Responses[401] = "Unauthorized";
        });
    }

    public override async Task HandleAsync(SetDefaultFavouriteRequest req, CancellationToken ct)
    {
        try
        {
            var favourite = await favouriteRepository.GetFavouriteByIdAsync(req.Id);

            if (favourite == null || favourite.UserId != userContext.UserId)
            {
                ThrowError("Favourite not found", 404);
            }

            // Remove default flag from all user's favourites
            await favouriteRepository.RemoveAllUserDefaultFlagsAsync(userContext.UserId);

            // Set this favourite as default
            favourite.IsDefault = true;
            await favouriteRepository.UpdateFavouriteAsync(favourite);

            await Send.OkAsync(ct);
        }
        catch (InvalidOperationException ex) when (ex.Message.Contains("not authenticated") || ex.Message.Contains("Email claim"))
        {
            await Send.UnauthorizedAsync(ct);
        }
    }
}
