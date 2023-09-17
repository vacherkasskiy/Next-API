using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NextAPI.Bll.Services.Interfaces;
using NextAPI.Dal.Entities;
using NextAPI.Exceptions.Message;
using NextAPI.Requests.Messages;

namespace NextAPI.Controllers;

[ApiController]
[Route("[controller]")]
[Authorize]
public class MessagesController : ControllerBase
{
    private readonly IMessagesService _service;

    public MessagesController(IMessagesService service)
    {
        _service = service;
    }
    
    [HttpGet]
    [Route("/messages/get_for/{userId}")]
    public async Task<IActionResult> GetUserMessages(int userId)
    {
        try
        {
            var currentUserId = int
                .Parse(User
                .FindFirstValue(ClaimTypes.NameIdentifier));
            
            return Ok(await _service.GetAllForUsersPair(currentUserId, userId));
        }
        catch (MessageAuthorOrReceiverNotFoundException e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpGet]
    [Route("/messages/get_latest_for/{userId}")]
    public async Task<IActionResult> GetLatestMessage(int userId)
    {
        try
        {
            var currentUserId = int
                .Parse(User
                .FindFirstValue(ClaimTypes.NameIdentifier));
            
            var latest = await _service.GetLatestForUsersPair(currentUserId, userId);
            return Ok(latest);
        }
        catch (MessageAuthorOrReceiverNotFoundException e)
        {
            return BadRequest(e.Message);
        }
        catch (InvalidOperationException e)
        {
            return Ok("");
        }
    }
    

    [HttpPost]
    [Route("/messages/add")]
    public async Task<IActionResult> AddMessage(AddMessageRequest request)
    {
        try
        {
            var currentUserId = int
                    .Parse(User
                    .FindFirstValue(ClaimTypes.NameIdentifier));
            
            await _service.Add(new Message
            {
                AuthorId = currentUserId,
                ReceiverId = request.ReceiverId,
                Text = request.Text,
            });
            return Ok("Message successfully sent");
        }
        catch (MessageAuthorOrReceiverNotFoundException e)
        {
            return BadRequest(e.Message);
        }
    }
}