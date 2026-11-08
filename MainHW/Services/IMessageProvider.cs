namespace MainHW.Services;

public interface IMessageProvider<T> where T : class
{
    public Task<ICollection<T>> GetAllAsync();

    public Task SaveAsync(ICollection<T> items);

}
