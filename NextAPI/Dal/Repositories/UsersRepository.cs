using Microsoft.EntityFrameworkCore;
using NextAPI.Dal.Entities;
using NextAPI.Dal.Repositories.Interfaces;

namespace NextAPI.Dal.Repositories;

public class UsersRepository : IBaseRepository<User>
{
    private readonly ApplicationDbContext _db;

    public UsersRepository(ApplicationDbContext db)
    {
        _db = db;
    }

    public async Task<User[]> GetAll()
    {
        return await _db.Users
            .OrderBy(x => x.Id)
            .ToArrayAsync();
    }

    public async Task<User?> GetById(int userId)
    {
        return await _db.Users.FindAsync(userId);
    }

    public async Task Update(User user)
    {
        _db.Users.Update(user);
        await _db.SaveChangesAsync();
    }

    public Task Add(User item)
    {
        throw new NotImplementedException();
    }
}