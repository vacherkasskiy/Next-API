namespace NextAPI.Requests.Auth;

public record RegisterRequest(
    string Name,
    string Username, 
    string Email,
    string Password);