using NextAPI.Bll.Services.Interfaces;
using NextAPI.Dal.Entities;
using NextAPI.Dal.Repositories.Interfaces;

namespace NextAPI.Bll.Services;

public class PostsService : IOrientedService<Post>
{
    private readonly IBaseRepository<Post> _repository;

    public PostsService(IBaseRepository<Post> repository)
    {
        _repository = repository;
    }

    public async Task<Post[]> GetForUser(int userId)
    {
        var posts = await _repository.GetAll();
        return posts.Where(x => x.ReceiverId == userId).ToArray();
    }

    public Task<Post[]> GetAll()
    {
        throw new NotImplementedException();
    }

    public Task<GetPartitionResponse<Post>> GetPartition(int skip, int limit)
    {
        throw new NotImplementedException();
    }

    public Task<Post> GetById(int id)
    {
        throw new NotImplementedException();
    }

    public Task Update(Post item)
    {
        throw new NotImplementedException();
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