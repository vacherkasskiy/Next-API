using Microsoft.AspNetCore.Mvc;
using NextAPI.Bll.Services;
using NextAPI.Requests.Users;

namespace NextAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class UsersController : ControllerBase
{
    private readonly UsersService _service;
    
    public UsersController(UsersService service)
    {
        _service = service;
    }

    [Route("/users")]
    [HttpGet]
    public IActionResult GetUsers([FromQuery]GetUsersRequest request)
    {
        return Ok(_service.GetAll(request.Limit, request.Skip));
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

    [Route("/users/setStatus")]
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