using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using NextAPI.Bll.Services.Interfaces;
using NextAPI.Dal.Entities;
using NextAPI.Requests.Auth;

namespace NextAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _service;

    public AuthController(IAuthService service)
    {
        _service = service;
    }
    
    [HttpPost]
    [Route("/auth/register")]
    public async Task<IActionResult> Register(RegisterRequest request)
    {
        try
        {
            await _service.Register(new User
            {
                Name = request.Name,
                Username = request.Username,
                Email = request.Email
            });

            return Ok();
        }
        catch
        {
            return BadRequest();
        }
    }

    [HttpPost]
    [Route("/auth/login")]
    public async Task<IActionResult> Login(LoginRequest request)
    {
        var principal = await _service.Login(request.Email, request.Password);
        await HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            principal);
        return Ok();
    }

    [HttpGet]
    [Route("/auth/get_current")]
    public IActionResult GetCurrent()
    {
        if (User.Identity!.IsAuthenticated)
        {
            return Ok(User.FindFirstValue(ClaimTypes.Email));
        }
        else
        {
            return BadRequest();
        }
    }
}