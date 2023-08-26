using NextAPI.Bll.Services.Interfaces;
using NextAPI.Dal.Entities;
using NextAPI.Dal.Repositories.Interfaces;

namespace NextAPI.Bll.Services;

public class UsersService : IBaseService<User>
{
    private readonly IBaseRepository<User> _repository;

    public UsersService(IBaseRepository<User> repository)
    {
        _repository = repository;
    }

    public async Task<User[]> GetAll()
    {
        return await _repository.GetAll();
    }

    public async Task<GetPartitionResponse<User>> GetPartition(int skip, int limit)
    {
        var users = await _repository.GetAll();
        return new GetPartitionResponse<User>(users
            .Skip(skip)
            .Take(limit)
            .ToArray(), users.Length);
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

    public Task<User> Add(User item)
    {
        throw new NotImplementedException();
    }
}