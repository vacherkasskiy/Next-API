using Microsoft.EntityFrameworkCore;
using NextAPI.Bll.Models;
using NextAPI.Dal.Entities;

namespace NextAPI.Dal.Repositories;

public class PostsRepository
{
    private readonly ApplicationDbContext _db;

    public PostsRepository(ApplicationDbContext db)
    {
        _db = db;
    }
    
    public async Task<Post[]> GetAll()
    {
        return await _db.Posts.Include(p => p.Author).Include(p => p.Receiver).ToArrayAsync();
    }

    public async Task<Post> Add(PostModel post)
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