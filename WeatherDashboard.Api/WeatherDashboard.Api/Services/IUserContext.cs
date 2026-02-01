namespace WeatherDashboard.Api.Services;

public interface IUserContext
{
    string Email { get; }
    Guid UserId { get; }
}
