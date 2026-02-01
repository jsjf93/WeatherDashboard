namespace WeatherDashboard.Api.Models.Entities;

public class User
{
    public Guid Id { get; set; }
    public string Email { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }

    public ICollection<UserFavourite> Favourites { get; set; } = new List<UserFavourite>();
}
