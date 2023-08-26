using NextAPI.Bll.Services.Interfaces;
using NextAPI.Dal.Entities;
using NextAPI.Dal.Repositories.Interfaces;

namespace NextAPI.Bll.Services;

public class MessagesService : IOrientedService<Message>
{
    private readonly IBaseRepository<Message> _messageRepository;
    private readonly IBaseRepository<User> _userRepository;

    public MessagesService(
        IBaseRepository<Message> messageRepository,
        IBaseRepository<User> userRepository)
    {
        _messageRepository = messageRepository;
        _userRepository = userRepository;
    }

    public async Task<Message[]> GetAll()
    {
        return await _messageRepository.GetAll();
    }

    public Task<GetPartitionResponse<Message>> GetPartition(int skip, int limit)
    {
        throw new NotImplementedException();
    }

    public Task<Message> GetById(int id)
    {
        throw new NotImplementedException();
    }

    public Task Update(Message item)
    {
        throw new NotImplementedException();
    }

    public async Task Add(Message message)
    {
        var author = await _userRepository.GetById(message.AuthorId);
        var receiver = await _userRepository.GetById(message.ReceiverId);
        
        if (author == null || receiver == null)
        {
            throw new ArgumentOutOfRangeException();
        }

        await _messageRepository.Add(message);
    }

    public async Task<Message[]> GetForUser(int userId)
    {
        var user = await _userRepository.GetById(userId);
        if (user == null)
        {
            throw new ArgumentOutOfRangeException();
        }

        var messages = await _messageRepository.GetAll();
        return messages
            .Where(x => x.ReceiverId == userId)
            .ToArray();
    }
}