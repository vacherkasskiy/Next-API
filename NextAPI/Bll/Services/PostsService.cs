using NextAPI.Dal.Entities;
using NextAPI.Dal.Repositories.Interfaces;

namespace NextAPI.Bll.Services;

public class PostsService
{
    private readonly IBaseRepository<Post> _repository;

    public PostsService(IBaseRepository<Post> repository)
    {
        _repository = repository;
    }

    public async Task<Post[]> GetUserPosts(int userId)
    {
        var posts = await _repository.GetAll();
        return posts.Where(x => x.ReceiverId == userId).ToArray();
    }

    public async Task<Post> Add(Post post)
    {
        var newPost = await _repository.Add(post);

        if (newPost.Author == null || newPost.Receiver == null)
        {
            throw new ArgumentOutOfRangeException();
        }

        return newPost;
    }
}