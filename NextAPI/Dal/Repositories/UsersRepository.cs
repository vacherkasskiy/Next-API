using NextAPI.Dal.Entities;
using NextAPI.Data;

namespace NextAPI.Dal.Repositories;

public class UsersRepository
{
    private readonly ApplicationDbContext _db;

    public UsersRepository(ApplicationDbContext db)
    {
        _db = db;
    }

    public User[] GetAll(int limit, int skip)
    {
        return _db.Users
            .Skip(skip)
            .Take(limit)
            .ToArray();
    }

    public User? GetById(int userId)
    {
        return _db.Users.Find(userId);
    }
}