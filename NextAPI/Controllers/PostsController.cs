using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NextAPI.Bll.Services.Interfaces;
using NextAPI.Dal.Entities;
using NextAPI.Exceptions.Post;
using NextAPI.Requests.Posts;

namespace NextAPI.Controllers;

[ApiController]
[Route("[controller]")]
[Authorize]
public class PostsController : ControllerBase
{
    private readonly IPostsService _service;

    public PostsController(IPostsService service)
    {
        _service = service;
    }

    [HttpGet]
    [Route("/posts/get_for/{userId}")]
    public async Task<IActionResult> GetUserPosts(int userId)
    {
        try
        {
            return Ok(await _service.GetForUser(userId));
        }
        catch (PostWithWrongReceiverIdException e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpPost]
    [Route("/posts/add")]
    public async Task<IActionResult> AddPost(AddPostRequest request)
    {
        try
        {
            var currentUserId = int
                .Parse(User
                .FindFirstValue(ClaimTypes.NameIdentifier)!);
            
            await _service.Add(new Post{
                AuthorId = currentUserId,
                ReceiverId = request.ReceiverId,
                Text = request.Text});
            return Ok("Post successfully added");
        }
        catch (PostAuthorOrReceiverNotFoundException e)
        {
            return BadRequest(e.Message);
        }
    }
}