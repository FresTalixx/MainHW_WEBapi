using Microsoft.EntityFrameworkCore;

namespace MainHW.Models;

public class ApiDbContext : DbContext
{
    public ApiDbContext(DbContextOptions<ApiDbContext> options) : base(options)
    {
    }
    public virtual DbSet<Message> Messages { get; set; }
    public virtual DbSet<User> Users { get; set; }
}
