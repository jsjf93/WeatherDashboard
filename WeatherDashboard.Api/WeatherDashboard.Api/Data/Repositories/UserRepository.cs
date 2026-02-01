using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using WeatherDashboard.Api.Models.Entities;

namespace WeatherDashboard.Api.Data.Repositories;

public class UserRepository(WeatherDashboardDbContext dbContext)
{
    public async Task<User> GetOrCreateUserByEmailAsync(string email)
    {
        var normalisedEmail = email.Trim().ToLower();
        var user = await dbContext.Users.FirstOrDefaultAsync(u => u.Email.Equals(normalisedEmail, StringComparison.CurrentCultureIgnoreCase));

        if (user == null)
        {
            user = new User
            {
                Id = Guid.NewGuid(),
                Email = normalisedEmail,
                CreatedAt = DateTime.UtcNow
            };

            dbContext.Users.Add(user);
            try
            {
                await dbContext.SaveChangesAsync();
            }
            catch (DbUpdateException ex) when (IsUniqueConstraintViolation(ex))
            {
                dbContext.Entry(user).State = EntityState.Detached;
                user = await dbContext.Users.FirstAsync(u => u.Email.Equals(normalisedEmail, StringComparison.CurrentCultureIgnoreCase));
            }
        }

        return user;
    }

    private static bool IsUniqueConstraintViolation(DbUpdateException ex)
    {
        if (ex.InnerException is not SqlException sqlException)
        {
            return false;
        }

        return sqlException.Number is 2601 or 2627;
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
