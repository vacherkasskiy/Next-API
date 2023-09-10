using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
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

    private static ClaimsPrincipal GetClaims(User user)
    {
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
        };
        
        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        var principal = new ClaimsPrincipal(identity);

        return principal;
    }
    
    public async Task<ClaimsPrincipal> Register(User user)
    {
        var sameEmailUsers = (await _repository.GetAll())
            .Where(x => x.Email == user.Email)
            .ToArray();

        if (sameEmailUsers.Length > 0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(user), "User with such email already exists");
        }
        
        var passwordHasher = new PasswordHasher<User>();
        user.Password = passwordHasher.HashPassword(user, user.Password);
        await _repository.Add(user);
        return GetClaims(user);
    }

    public async Task<ClaimsPrincipal> Login(string email, string password)
    {
        var user = (await _repository
            .GetAll()).Single(x => x.Email == email);

        var passwordHasher = new PasswordHasher<User>();
        var result = passwordHasher.VerifyHashedPassword(user, user.Password, password);

        if (result == PasswordVerificationResult.Failed)
        {
            throw new ArgumentOutOfRangeException(nameof(user), "Wrong password");
        }
        
        return GetClaims(user);
    }
}