using Microsoft.EntityFrameworkCore;
using NextAPI.Dal.Entities;
using NextAPI.Dal.Repositories.Interfaces;

namespace NextAPI.Dal.Repositories;

public class PostsRepository : IBaseRepository<Post>
{
    private readonly ApplicationDbContext _db;

    public PostsRepository(ApplicationDbContext db)
    {
        _db = db;
    }
    
    public async Task<Post[]> GetAll()
    {
        return await _db.Posts
            .Include(p => p.Author)
            .Include(p => p.Receiver)
            .ToArrayAsync();
    }

    public Task<Post?> GetById(int id)
    {
        throw new NotImplementedException();
    }

    public Task Update(Post item)
    {
        throw new NotImplementedException();
    }

    public async Task<Post> Add(Post post)
    {
        var newPost = await _db.Posts.AddAsync(new Post
        {
            AuthorId = post.AuthorId,
            ReceiverId = post.ReceiverId,
            Text = post.Text,
            Author = await _db.Users.FindAsync(post.AuthorId),
            Receiver = await _db.Users.FindAsync(post.ReceiverId),
        });
        await _db.SaveChangesAsync();

        return newPost.Entity;
    }
}