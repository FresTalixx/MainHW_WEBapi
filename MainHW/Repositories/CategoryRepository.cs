using MainHW.Models;

namespace MainHW.Repositories;

public class CategoryRepository(ApiDbContext context) : BaseRepository<Category>(context)
{
}
