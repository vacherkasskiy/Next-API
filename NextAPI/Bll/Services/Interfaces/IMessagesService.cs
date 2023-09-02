using NextAPI.Dal.Entities;

namespace NextAPI.Bll.Services.Interfaces;

public interface IMessagesService : IBaseService<Message>
{
    Task<Message[]> GetAllForUsersPair(int firstId, int secondId);
    Task<Message> GetLatestForUsersPair(int firstId, int secondId);
}