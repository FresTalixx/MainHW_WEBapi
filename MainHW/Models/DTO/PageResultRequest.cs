using System.ComponentModel.DataAnnotations;

namespace MainHW.Models.DTO;

public class PageResultRequest<T> where T : class
{
    [Range(1, int.MaxValue, ErrorMessage = "Page number must be greater than 0.")]
    public int PageNumber { get; set; } = 1;
    [Range(1, int.MaxValue, ErrorMessage = "Page size must be greater than 0.")]
    public int PageSize { get; set; } = 10;
    public string? OrderBy { get; set; } = null;
    public T? Filter { get; set; } = null;

}
