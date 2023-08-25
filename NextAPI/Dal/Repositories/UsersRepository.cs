using NextAPI.Dal.Entities;

namespace NextAPI.Dal.Repositories;

public class UsersRepository
{
    private readonly ApplicationDbContext _db;

    public UsersRepository(ApplicationDbContext db)
    {
        _db = db;
    }

    public User[] GetAll()
    {
        return _db.Users
            .OrderBy(x => x.Id)
            .ToArray();
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
}