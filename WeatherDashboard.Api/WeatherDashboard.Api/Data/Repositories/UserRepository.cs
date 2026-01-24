using Microsoft.EntityFrameworkCore;
using WeatherDashboard.Api.Models.Entities;

namespace WeatherDashboard.Api.Data.Repositories;

public class UserRepository(WeatherDashboardDbContext dbContext)
{
    public async Task<User> GetOrCreateUserByEmailAsync(string email)
    {
        var user = await dbContext.Users.FirstOrDefaultAsync(u => u.Email == email);

        if (user == null)
        {
            user = new User
            {
                Id = Guid.NewGuid(),
                Email = email,
                CreatedAt = DateTime.UtcNow
            };

            dbContext.Users.Add(user);
            await dbContext.SaveChangesAsync();
        }

        return user;
    }

    public async Task<User?> GetUserByIdAsync(Guid userId)
    {
        return await dbContext.Users.FirstOrDefaultAsync(u => u.Id == userId);
    }

    public async Task<User?> GetUserByEmailAsync(string email)
    {
        return await dbContext.Users.FirstOrDefaultAsync(u => u.Email == email);
    }
}
