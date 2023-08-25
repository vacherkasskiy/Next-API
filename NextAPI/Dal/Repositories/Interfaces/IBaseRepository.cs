namespace NextAPI.Dal.Repositories.Interfaces;

public interface IBaseRepository<T>
{
    Task<T[]> GetAll();
    Task<T?> GetById(int id);
    Task Update(T item);
    Task<T> Add(T item);
}