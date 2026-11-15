namespace MainHW.Models.DTO;

public class PaginetadResult<T> where T : class
{
    public int TotalCount { get; set; }
    public int PageSize { get; set; }
    public int CurrentPage { get; set; }
    public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
    public ICollection<T> Models { get; set; } = null!;
}
