using Microsoft.AspNetCore.Mvc;
using NextAPI.Bll.Services.Interfaces;
using NextAPI.Dal.Entities;
using NextAPI.Exceptions.User;
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
        var response = await _service.GetPartition(
            request.Skip,
            request.Limit);
        
        return Ok(new GetUsersResponse
        (
            response.Entities,
            response.Length
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
        catch (UserNotFoundByIdException e)
        {
            return BadRequest(e.Message);
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
        catch (UserNotFoundByIdException e)
        {
            return BadRequest(e.Message);
        }
    }
}