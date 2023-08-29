using Microsoft.AspNetCore.Mvc;
using NextAPI.Bll.Services.Interfaces;
using NextAPI.Dal.Entities;
using NextAPI.Requests.Messages;

namespace NextAPI.Controllers;

[ApiController]
[Route("[controller]")]
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
            return Ok(await _service.GetForUser(userId));
        }
        catch
        {
            return BadRequest("Wrong user Id");
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