using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NextAPI.Bll.Services.Interfaces;
using NextAPI.Dal.Entities;
using NextAPI.Exceptions;
using NextAPI.Exceptions.User;
using NextAPI.Requests.Users;
using NextAPI.Responses.Users;

namespace NextAPI.Controllers;

[ApiController]
[Route("[controller]")]
[Authorize]
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

    [Route("/users/current/set_status")]
    [HttpPatch]
    public async Task<IActionResult> SetStatus(string status)
    {
        try
        {
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            var currentUser = await _service.GetById(int.Parse(currentUserId));
            
            currentUser.Status = status;
            await _service.Update(currentUser);
            return Ok();
        }
        catch (UserNotFoundByIdException e)
        {
            return BadRequest(e.Message);
        }
    }

    [Route("/users/current/edit")]
    [HttpPatch]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(403)]
    public async Task<IActionResult> EditProfile(EditProfileRequest request)
    {
        try
        {
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            var currentUser = await _service.GetById(int.Parse(currentUserId));

            currentUser.Image = request.Image;
            currentUser.Name = request.Name;
            currentUser.Username = request.Username;
            currentUser.Email = request.Email;
            currentUser.City = request.City;
            currentUser.Website = request.Website;
            await _service.Update(currentUser);
        
            return Ok("Updated successfully");
        }
        catch (UserAlreadyExistsException e)
        {
            return BadRequest(e.Message);
        }
        catch (InvalidAvatarImageLinkException e)
        {
            return StatusCode(StatusCodes.Status403Forbidden, e.Message);
        }
    }
}