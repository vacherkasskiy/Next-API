namespace NextAPI.Bll.Services.Interfaces;

public interface IOrientedService<T> : IBaseService<T>
{
    Task<T[]> GetForUser(int userId);
}