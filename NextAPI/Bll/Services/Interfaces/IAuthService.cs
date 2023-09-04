using System.Security.Claims;
using NextAPI.Dal.Entities;

namespace NextAPI.Bll.Services.Interfaces;

public interface IAuthService
{
    Task Register(User user);
    Task<ClaimsPrincipal> Login(string email, string password);
    Task Logout(User user);
}