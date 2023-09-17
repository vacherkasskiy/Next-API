using NextAPI.Bll.Services.Interfaces;
using NextAPI.Dal.Entities;
using NextAPI.Dal.Repositories.Interfaces;
using NextAPI.Exceptions;
using NextAPI.Exceptions.User;

namespace NextAPI.Bll.Services;

public class UsersService : IBaseService<User>
{
    private readonly IBaseRepository<User> _repository;
    
    private static bool IsImageUrl(string url)
    {
        using var client = new HttpClient();
        var response = client.Send(new HttpRequestMessage(HttpMethod.Head, url));
        return response.Content.Headers.ContentType!.MediaType!
            .StartsWith("image/", StringComparison.OrdinalIgnoreCase);
    }

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
        var user = await _repository.GetById(userId);
        if (user == null)
        {
            throw new UserNotFoundByIdException();
        }

        return user;
    }
    
    public async Task Update(User user)
    {
        var usersWithSameEmail = (await _repository
                .GetAll())
            .Where(x => x.Email == user.Email)
            .ToArray();

        if (usersWithSameEmail.Length > 1)
        {
            throw new UserAlreadyExistsException();
        }

        await _repository.Update(user);
    }

    public async Task Add(User user)
    {
        await _repository.Add(user);
    }

    public async Task DeleteById(int userId)
    {
        var user = await _repository.GetById(userId);
        if (user == null)
        {
            throw new UserNotFoundByIdException();
        }

        await _repository.Delete(user);
    }
}