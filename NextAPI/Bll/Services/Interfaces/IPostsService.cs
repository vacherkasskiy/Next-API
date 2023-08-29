using NextAPI.Dal.Entities;

namespace NextAPI.Bll.Services.Interfaces;

public interface IPostsService : IBaseService<Post>
{
    Task<Post[]> GetForUser(int userId);
}