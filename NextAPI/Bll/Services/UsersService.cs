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

    public async Task<User> GetById(int userId)
    {
        User? user = await _repository.GetById(userId);
        if (user == null)
        {
            throw new ArgumentOutOfRangeException();
        }

        return user;
    }

    public async Task Update(User user)
    {
        await _repository.Update(user);
    }
}