namespace NextAPI.Requests.Users;

public record EditProfileRequest(
    string? Image,
    string Name,
    string Username,
    string Email,
    string? City,
    string? Website);