using MainHW.Models;
using Microsoft.EntityFrameworkCore;

namespace MainHW.Services;

public class MessageRepository(ApiDbContext context) : IRepository<Message>
{
    public void Add(Message message)
    {
        context.Messages.Add(message);
    }

    public async Task<ICollection<Message>> GetAllAsync()
    {
        return await context.Messages.ToListAsync();
    }

    public async Task<Message?> GetByIdAsync(int id)
    {
        return await context.Messages.FindAsync(id);
    }

    public void Remove(Message message)
    {
        context.Messages.Remove(message);
    }

    public async Task SaveChangesAsync()
    {
        await context.SaveChangesAsync();
    }
}
