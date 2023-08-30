namespace NextAPI.Bll.Services.Interfaces;

public record GetPartitionResponse<T>(T[] Entities, int Length);

public interface IBaseService<T>
{
    Task<T[]> GetAll();
    Task<GetPartitionResponse<T>> GetPartition(int skip, int limit);
    Task<T> GetById(int id);
    Task Update(T item);
    Task Add(T item);
    Task DeleteById(int id);
}