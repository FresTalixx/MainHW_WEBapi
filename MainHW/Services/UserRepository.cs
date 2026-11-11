namespace MainHW.Services;

using MainHW.Models;
using Microsoft.EntityFrameworkCore;

public class UserRepository(ApiDbContext context) : IRepository<User>
{
    public async Task<ICollection<User>> GetAllAsync()
    {
        return await context.Users.ToListAsync();
    }
    public async Task<User?> GetByIdAsync(int id)
    {
        return await context.Users.FindAsync(id);
    }
    public void Add(User entity)
    {
        context.Users.Add(entity);
    }
    public void Remove(User entity)
    {
        context.Users.Remove(entity);
    }
    public async Task SaveChangesAsync()
    {
        await context.SaveChangesAsync();
    }
}

