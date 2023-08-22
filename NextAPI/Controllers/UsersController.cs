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
    public IActionResult GetUser(int userId)
    {
        try
        {
            return Ok(_service.GetById(userId));
        }
        catch
        {
            return BadRequest("Wrong user id");
        }
    }
}