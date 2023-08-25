namespace NextAPI.Bll.Services.Interfaces;

public interface IBaseService<T>
{
    Task<T[]> GetAll();
    Task<T> GetById(int id);
    Task Update(T item);
    Task<T> Add(T item);
}