namespace MainHW.Models.DTO;

public class OrderDto
{
    /// <summary>
    /// Gets or sets the identifier.
    /// </summary>
    public int Id { get; set; }
    
    /// <summary>
    /// Gets or sets the date and time.
    /// </summary>
    public DateTime DateTime { get; set; }
    
    /// <summary>
    /// Gets or sets the quantity.
    /// </summary>
    public int Quantity { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the operation has completed.
    /// </summary>
    public bool IsCompleted { get; set; }

    /// <summary>
    /// Gets or sets the total cost.
    /// </summary>
    public int TotalCost { get; set; }
}
