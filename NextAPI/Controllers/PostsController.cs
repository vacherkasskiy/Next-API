using Microsoft.AspNetCore.Mvc;
using NextAPI.Bll.Models;
using NextAPI.Bll.Services;
using NextAPI.Dal.Entities;
using NextAPI.Requests.Posts;

namespace NextAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class PostsController : ControllerBase
{
    private readonly PostsService _service;

    public PostsController(PostsService service)
    {
        _service = service;
    }

    [HttpGet]
    [Route("/posts/get_for/{userId}")]
    public async Task<IActionResult> GetUserPosts(int userId)
    {
        return Ok(await _service.GetUserPosts(userId));
    }

    [HttpPost]
    [Route("/posts/add")]
    public async Task<IActionResult> AddPost(AddPostRequest request)
    {
        try
        {
            var post = await _service.Add(new Post{
                AuthorId = request.AuthorId,
                ReceiverId = request.ReceiverId,
                Text = request.Text});
            return Ok();
        }
        catch
        {
            return BadRequest("Wrong author or/and receiver Id");
        }
    }
}