using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using NextAPI.Bll.Services.Interfaces;
using NextAPI.Dal.Entities;
using NextAPI.Requests.Auth;

namespace NextAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly IBaseService<User> _userService;

    public AuthController(
        IAuthService authService,
        IBaseService<User> userService)
    {
        _authService = authService;
        _userService = userService;
    }
    
    [HttpGet]
    [Route("/auth/get_current")]
    public async Task<IActionResult> GetCurrent()
    {
        if (User.Identity!.IsAuthenticated)
        {
            var id = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            var user = await _userService.GetById(int.Parse(id));
            return Ok(user);
        }
        return BadRequest();
    }
    
    [HttpPost]
    [Route("/auth/register")]
    public async Task<IActionResult> Register(RegisterRequest request)
    {
        try
        {
            var principal = await _authService.Register(new User
            {
                Name = request.Name,
                Username = request.Username,
                Email = request.Email,
                Password = request.Password
            });
            
            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                principal);

            return Ok("Registered");
        }
        catch (ArgumentOutOfRangeException e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpPost]
    [Route("/auth/login")]
    public async Task<IActionResult> Login(LoginRequest request)
    {
        try
        {
            var principal = await _authService.Login(request.Email, request.Password);
            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                principal);
            
            return Ok("Authorized");
        }
        catch (InvalidOperationException e)
        {
            return BadRequest("Wrong email");
        }
        catch (ArgumentOutOfRangeException e)
        {
            return BadRequest("Wrong password");
        }
    }

    [Route("/auth/logout")]
    [HttpDelete]
    public async Task Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
    }
}