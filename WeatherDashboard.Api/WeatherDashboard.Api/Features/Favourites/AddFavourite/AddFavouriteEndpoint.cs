using FastEndpoints;
using WeatherDashboard.Api.Data.Repositories;
using WeatherDashboard.Api.Models.Entities;
using WeatherDashboard.Api.Services;

namespace WeatherDashboard.Api.Features.Favourites.AddFavourite;

public class AddFavouriteEndpoint(IUserContext userContext, UserFavouriteRepository favouriteRepository)
    : Endpoint<AddFavouriteRequest, AddFavouriteResponse>
{
    public override void Configure()
    {
        Post("/favourites");
        Summary(s =>
        {
            s.Summary = "Add a favourite city";
            s.Description = "Adds a new favourite city for the authenticated user";
            s.Responses[200] = "Successfully added favourite";
            s.Responses[400] = "Bad request (validation error)";
            s.Responses[401] = "Unauthorized";
        });
    }

    public override async Task HandleAsync(AddFavouriteRequest req, CancellationToken ct)
    {
        try
        {
            var existingFavourite = await favouriteRepository.GetFavouriteByUserAndCityAsync(
                userContext.UserId, req.City);

            if (existingFavourite != null)
            {
                AddError(r => r.City, "This city is already in your favourites", "favourite.exists");
                await Send.ErrorsAsync(statusCode: 409, cancellation: ct);
                return;
            }

            var favourite = new UserFavourite
            {
                Id = Guid.NewGuid(),
                UserId = userContext.UserId,
                City = req.City,
                IsDefault = false,
                CreatedAt = DateTime.UtcNow
            };

            await favouriteRepository.AddFavouriteAsync(favourite);

            var response = new AddFavouriteResponse
            {
                Id = favourite.Id,
                City = favourite.City,
                IsDefault = favourite.IsDefault,
                CreatedAt = favourite.CreatedAt
            };

            await Send.CreatedAtAsync<AddFavouriteEndpoint>(null, response, cancellation: ct);
        }
        catch (InvalidOperationException ex) when (ex.Message.Contains("not authenticated") || ex.Message.Contains("Email claim") || ex.Message.Contains("HttpContext"))
        {
            await Send.UnauthorizedAsync(ct);
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Error adding favourite for user {UserId}", userContext.UserId);
            AddError("Failed to add favourite", "favourite.add_failed");
            await Send.ErrorsAsync(statusCode: 500, cancellation: ct);
        }
    }
}
