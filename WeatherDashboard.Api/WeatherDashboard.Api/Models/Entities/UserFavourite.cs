namespace WeatherDashboard.Api.Models.Entities;

public class UserFavourite
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string City { get; set; } = string.Empty;
    public bool IsDefault { get; set; }
    public DateTime CreatedAt { get; set; }

    public User User { get; set; } = null!;
}
