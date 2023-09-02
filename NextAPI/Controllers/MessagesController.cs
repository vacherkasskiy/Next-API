using Microsoft.AspNetCore.Mvc;
using NextAPI.Bll.Services.Interfaces;
using NextAPI.Dal.Entities;
using NextAPI.Requests.Messages;

namespace NextAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class MessagesController : ControllerBase
{
    /// <summary>
    /// Заглушка, отображаящая Id пользователя,
    /// будто бы находящегося в сессии на данный момент времени.
    /// </summary>
    private readonly int _currentUserId = 1;
    
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
            return Ok(await _service.GetAllForUsersPair(_currentUserId, userId));
        }
        catch
        {
            return BadRequest("Wrong user Id");
        }
    }

    [HttpGet]
    [Route("/messages/get_latest/{userId}")]
    public async Task<IActionResult> GetLatestMessage(int userId)
    {
        try
        {
            var latest = await _service.GetLatestForUsersPair(_currentUserId, userId);
            return Ok(latest);
        }
        catch (ArgumentOutOfRangeException e)
        {
            return BadRequest("Wrong user Id");
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
            await _service.Add(new Message
            {
                AuthorId = request.AuthorId,
                ReceiverId = request.ReceiverId,
                Text = request.Text,
            });
            return Ok("Message successfully sent");
        }
        catch
        {
            return BadRequest("Wrong author or/and receiver Id");
        }
    }
}