namespace MainHW.Repositories;

using MainHW.Models;
using Microsoft.EntityFrameworkCore;

public class UserRepository(ApiDbContext context) : BaseRepository<User>(context)
{
}

