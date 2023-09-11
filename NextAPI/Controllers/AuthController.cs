using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NextAPI.Bll.Services.Interfaces;
using NextAPI.Dal.Entities;
using NextAPI.Exceptions.Auth;
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
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> GetCurrent()
    {
        if (!User.Identity!.IsAuthenticated)
        {
            return BadRequest();
        }
        
        var id = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        var user = await _userService.GetById(int.Parse(id));
        return Ok(user);
    }
    
    [HttpPost]
    [Route("/auth/register")]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
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
        catch (UserAlreadyExistsException e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpPost]
    [Route("/auth/login")]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(403)]
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
        catch (WrongEmailException e)
        {
            return BadRequest(e.Message);
        }
        catch (WrongPasswordException e)
        {
            return StatusCode(StatusCodes.Status403Forbidden, e.Message);
        }
    }

    [Authorize]
    [Route("/auth/logout")]
    [HttpDelete]
    [ProducesResponseType(400)]
    [ProducesResponseType(405)]
    public async Task Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
    }
}