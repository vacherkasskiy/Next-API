using NextAPI.Bll.Services.Interfaces;
using NextAPI.Dal.Entities;
using NextAPI.Dal.Repositories.Interfaces;

namespace NextAPI.Bll.Services;

public class MessagesService : IMessagesService
{
    private readonly IBaseRepository<Message> _messageRepository;
    private readonly IBaseRepository<User> _userRepository;
    
    /// <summary>
    /// Заглушка, отображаящая Id пользователя,
    /// будто бы находящегося в сессии на данный момент времени.
    /// </summary>
    private readonly int _currentUserId = 1;

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

    private bool CheckIfIncluded(int userId, Message message)
    {
        return (message.ReceiverId == userId && message.AuthorId == _currentUserId) ||
               (message.ReceiverId == _currentUserId && message.AuthorId == userId);
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
            .OrderBy(x => x.Id)
            .Where(x => CheckIfIncluded(userId, x))
            .ToArray();
    }
}