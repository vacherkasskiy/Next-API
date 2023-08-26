using NextAPI.Bll.Services.Interfaces;
using NextAPI.Dal.Entities;
using NextAPI.Dal.Repositories.Interfaces;

namespace NextAPI.Bll.Services;

public class PostsService : IOrientedService<Post>
{
    private readonly IBaseRepository<Post> _postRepository;
    private readonly IBaseRepository<User> _userRepository;

    public PostsService(
        IBaseRepository<Post> postRepository,
        IBaseRepository<User> userRepository)
    {
        _postRepository = postRepository;
        _userRepository = userRepository;
    }

    public async Task<Post[]> GetForUser(int userId)
    {
        var user = await _userRepository.GetById(userId);
        if (user == null)
        {
            throw new ArgumentOutOfRangeException();
        }
        
        var posts = await _postRepository.GetAll();
        return posts
            .Where(x => x.ReceiverId == userId)
            .ToArray();
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

    public async Task Add(Post post)
    {
        var author = await _userRepository.GetById(post.AuthorId);
        var receiver = await _userRepository.GetById(post.ReceiverId);

        if (author == null || receiver == null)
        {
            throw new ArgumentOutOfRangeException();
        }
        
        await _postRepository.Add(post);
    }
}