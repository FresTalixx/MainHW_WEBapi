using System.ComponentModel.DataAnnotations;

namespace MainHW.Models;

/// <summary>
/// Represents a category.
/// </summary>
public class Category
{
    /// <summary>
    /// Gets or sets the unique identifier.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the name.
    /// </summary>
    /// <remarks>The value must be between 3 and 50 characters long.</remarks>
    public string Name { get; set; } = null!;
    /// <summary>
    /// Gets or sets the creation date.
    /// </summary>
    public DateTime CreationDate { get; set; } = DateTime.Now;

    /// <summary>
    /// Gets or sets the date and time when the entity was last updated.
    /// </summary>
    public DateTime? UpdateDate { get; set; }

    public string? BgFileName { get; set; } // New property to store the background file name

}
