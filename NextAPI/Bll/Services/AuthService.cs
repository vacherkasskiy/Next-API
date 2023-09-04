using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using NextAPI.Bll.Services.Interfaces;
using NextAPI.Dal.Entities;
using NextAPI.Dal.Repositories.Interfaces;

namespace NextAPI.Bll.Services;

public class AuthService : IAuthService
{
    private readonly IBaseRepository<User> _repository;

    public AuthService(IBaseRepository<User> repository)
    {
        _repository = repository;
    }

    public async Task Register(User user)
    {
        var sameEmailUsers = (await _repository
                .GetAll())
            .Where(x => x.Email == user.Email)
            .ToArray();

        if (sameEmailUsers.Length > 0)
        {
            throw new ArgumentOutOfRangeException();
        }

        await _repository.Add(user);
    }

    public async Task<ClaimsPrincipal> Login(string email, string password)
    {
        var user = (await _repository
            .GetAll()).Single(x => x.Email == email);

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Name),
            new Claim(ClaimTypes.Email, user.Email)
        };
        
        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        var principal = new ClaimsPrincipal(identity);

        return principal;
    }

    public Task Logout(User user)
    {
        throw new NotImplementedException();
    }
}