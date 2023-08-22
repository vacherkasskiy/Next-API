using NextAPI.Dal.Entities;
using NextAPI.Dal.Repositories;

namespace NextAPI.Bll.Services;

public class UsersService
{
    private readonly UsersRepository _repository;

    public UsersService(UsersRepository repository)
    {
        _repository = repository;
    }

    public User[] GetAll(int limit, int skip)
    {
        return _repository.GetAll(limit, skip);
    }

    public User GetById(int userId)
    {
        User? user = _repository.GetById(userId);
        if (user == null)
        {
            throw new ArgumentOutOfRangeException();
        }

        return user;
    }
}