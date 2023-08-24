using NextAPI.Bll.Models;
using NextAPI.Dal.Entities;
using NextAPI.Dal.Repositories;

namespace NextAPI.Bll.Services;

public class PostsService
{
    private readonly PostsRepository _repository;

    public PostsService(PostsRepository repository)
    {
        _repository = repository;
    }

    public async Task<Post[]> GetUserPosts(int userId)
    {
        var posts = await _repository.GetAll();
        return posts.Where(x => x.ReceiverId == userId).ToArray();
    }

    public async Task<Post> Add(PostModel post)
    {
        var newPost = await _repository.Add(post);

        if (newPost.Author == null || newPost.Receiver == null)
        {
            throw new ArgumentOutOfRangeException();
        }

        return newPost;
    }
}