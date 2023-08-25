using Microsoft.AspNetCore.Mvc;
using NextAPI.Bll.Services.Interfaces;
using NextAPI.Dal.Entities;
using NextAPI.Requests.Users;
using NextAPI.Responses.Users;

namespace NextAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class UsersController : ControllerBase
{
    private readonly IBaseService<User> _service;
    
    public UsersController(IBaseService<User> service)
    {
        _service = service;
    }

    [Route("/users")]
    [HttpGet]
    public async Task<IActionResult> GetUsers([FromQuery]GetUsersRequest request)
    {
        var users = await _service.GetAll();
        return Ok(new GetUsersResponse
        (
            users
                .Skip(request.Skip)
                .Take(request.Limit)
                .ToArray(),
            users.Length
        ));
    }
    
    [Route("/users/{userId}")]
    [HttpGet]
    public async Task<IActionResult> GetUser(int userId)
    {
        try
        {
            return Ok(await _service.GetById(userId));
        }
        catch
        {
            return BadRequest("Wrong user id");
        }
    }

    [Route("/users/set_status")]
    [HttpPatch]
    public async Task<IActionResult> SetStatus(SetStatusRequest request)
    {
        try
        {
            var user = await _service.GetById(request.UserId);
            user.Status = request.Status;
            await _service.Update(user);
            return Ok();
        }
        catch
        {
            return BadRequest("Wrong user id");
        }
    }
}