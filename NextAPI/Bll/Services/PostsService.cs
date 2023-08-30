using NextAPI.Bll.Services.Interfaces;
using NextAPI.Dal.Entities;
using NextAPI.Dal.Repositories.Interfaces;

namespace NextAPI.Bll.Services;

public class PostsService : IPostsService
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

    public async Task<Post[]> GetAll()
    {
        return await _postRepository.GetAll();
    }

    public async Task<GetPartitionResponse<Post>> GetPartition(int skip, int limit)
    {
        var posts = await _postRepository.GetAll();
        return new GetPartitionResponse<Post>(posts
            .Skip(skip)
            .Take(limit)
            .ToArray(), posts.Length);
    }

    public async Task<Post> GetById(int postId)
    {
        var post = await _postRepository.GetById(postId);
        if (post == null)
        {
            throw new ArgumentOutOfRangeException();
        }

        return post;
    }

    public async Task Update(Post post)
    {
        await _postRepository.Update(post);
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

    public async Task DeleteById(int postId)
    {
        var post = await _postRepository.GetById(postId);
        if (post == null)
        {
            throw new ArgumentOutOfRangeException();
        }

        await _postRepository.Delete(post);
    }
}