using NextAPI.Dal.Entities;

namespace NextAPI.Responses.Users;

public record GetUsersResponse(User[] Users, int Length);