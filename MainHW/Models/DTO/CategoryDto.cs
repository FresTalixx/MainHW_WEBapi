namespace MainHW.Models.DTO;

public class CategoryDto
{
    /// <summary>
    /// Gets or sets the name.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    public IFormFile? File { get; set; }
}
