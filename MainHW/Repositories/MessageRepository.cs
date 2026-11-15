using MainHW.Models;
using Microsoft.EntityFrameworkCore;

namespace MainHW.Repositories;

public class MessageRepository(ApiDbContext context) : BaseRepository<Message>(context)
{
}
