using MainHW.Models.DTO;

namespace MainHW.Repositories;

public interface IRepository<T> where T : class
{
    Task<ICollection<T>> GetAllAsync();
    Task<PaginetadResult<T>> GetPaginatedAsync(
        int pageNumber, 
        int pageSize,
        Func<IQueryable<T>, IQueryable<T>>? orderBy,
        Func<IEnumerable<T>, IEnumerable<T>>? filter
    );
    Task<T?> GetByIdAsync(int id);
    void Add(T entity);
    void Remove(T entity);
    Task SaveChangesAsync();
}