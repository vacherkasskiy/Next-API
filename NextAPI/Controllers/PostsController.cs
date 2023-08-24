﻿using Microsoft.AspNetCore.Mvc;
using NextAPI.Bll.Models;
using NextAPI.Bll.Services;
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
    [Route("/posts/get_for/")]
    public async Task<IActionResult> GetUserPosts([FromQuery] int userId)
    {
        return Ok(await _service.GetUserPosts(userId));
    }

    [HttpPost]
    [Route("/posts/add")]
    public async Task<IActionResult> AddPost(AddPostRequest request)
    {
        try
        {
            var post = await _service.Add(new PostModel(
                request.AuthorId,
                request.ReceiverId,
                request.Text));
            return Ok();
        }
        catch
        {
            return BadRequest("Wrong author or/and receiver Id");
        }
    }
}