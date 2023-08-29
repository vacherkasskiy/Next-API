using NextAPI.Dal.Entities;

namespace NextAPI.Bll.Services.Interfaces;

public interface IMessagesService : IBaseService<Message>
{
    Task<Message[]> GetForUser(int userId);
}