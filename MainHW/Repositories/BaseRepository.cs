using MainHW.Models;
using MainHW.Models.DTO;
using Microsoft.EntityFrameworkCore;

namespace MainHW.Repositories;

abstract public class BaseRepository<T>(ApiDbContext context) : IRepository<T> where T : class
{
    public async Task<ICollection<T>> GetAllAsync()
    {
        return await context.Set<T>().ToListAsync();
    }

    public async Task<PaginetadResult<T>> GetPaginatedAsync(int pageNumber, int pageSize, Func<IQueryable<T>, IQueryable<T>>? orderBy = null, Func<IEnumerable<T>, IEnumerable<T>>? filter = null)
    {
        var query = context.Set<T>().AsQueryable();

        if (orderBy != null)
        {
            query = orderBy(query);
        }

        if (filter != null)
        {
            query = filter(query).AsQueryable();
        }

        var totalCount = await query.CountAsync();
        var models = await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
        return new PaginetadResult<T>
        {
            TotalCount = totalCount,
            PageSize = pageSize,
            CurrentPage = pageNumber,
            Models = models
        };
    }
    public async Task<T?> GetByIdAsync(int id)
    {
        return await context.Set<T>().FindAsync(id);
    }

    
    public void Add(T entity)
    {
        context.Set<T>().Add(entity);
    }
    public void Remove(T entity)
    {
        context.Set<T>().Remove(entity);
    }
    public async Task SaveChangesAsync()
    {
        await context.SaveChangesAsync();
    }
}
