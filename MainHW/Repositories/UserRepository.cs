namespace MainHW.Repositories;

using MainHW.Models;
using Microsoft.EntityFrameworkCore;

public class UserRepository(ApiDbContext context) : BaseRepository<User>(context)
{
    public async Task<User?> GetByEmailAsync(string email)
    {
        var entity = await context.Set<User>().FirstOrDefaultAsync(e => EF.Property<string>(e, "Email") == email);
        return entity;
    }
}

