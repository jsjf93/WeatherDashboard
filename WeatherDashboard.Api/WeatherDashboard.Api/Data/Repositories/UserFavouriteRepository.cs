using Microsoft.EntityFrameworkCore;
using WeatherDashboard.Api.Models.Entities;

namespace WeatherDashboard.Api.Data.Repositories;

public class UserFavouriteRepository(WeatherDashboardDbContext dbContext)
{
    public async Task<List<UserFavourite>> GetUserFavouritesAsync(Guid userId)
    {
        return await dbContext.UserFavourites
            .Where(f => f.UserId == userId)
            .OrderBy(f => f.City)
            .ToListAsync();
    }

    public async Task<UserFavourite?> GetFavouriteByIdAsync(Guid id)
    {
        return await dbContext.UserFavourites.FirstOrDefaultAsync(f => f.Id == id);
    }

    public async Task<UserFavourite?> GetFavouriteByUserAndCityAsync(Guid userId, string city)
    {
        return await dbContext.UserFavourites
            .FirstOrDefaultAsync(f => f.UserId == userId && f.City.ToLower() == city.ToLower());
    }

    public async Task AddFavouriteAsync(UserFavourite favourite)
    {
        dbContext.UserFavourites.Add(favourite);
        await dbContext.SaveChangesAsync();
    }

    public async Task UpdateFavouriteAsync(UserFavourite favourite)
    {
        dbContext.UserFavourites.Update(favourite);
        await dbContext.SaveChangesAsync();
    }

    public async Task RemoveFavouriteAsync(Guid id)
    {
        var favourite = await dbContext.UserFavourites.FirstOrDefaultAsync(f => f.Id == id);
        if (favourite != null)
        {
            dbContext.UserFavourites.Remove(favourite);
            await dbContext.SaveChangesAsync();
        }
    }

}
