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

    public async Task Add(Post post)
    {
        await _db.Posts.AddAsync(post);
        await _db.SaveChangesAsync();
    }
}