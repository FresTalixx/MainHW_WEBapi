using System.ComponentModel.DataAnnotations;

namespace MainHW.Models;

//Створіть модель замовлення з полями:
//номер замовлення;
//дата замовлення;
//кількість товарів;
//вартість товарів;
//дата-час замовлення;
//виконано(так/ні);
//коментар.

//Номер замовлення, дату-час замовлення і позначку «виконано» заповнює сервер.Нове замовлення завжди невиконане.

///<summary>
/// Represents an order.
/// </summary>
public class Order
{
    /// <summary>
    /// Gets or sets the order number.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the date and time when the order was placed.
    /// </summary>
    public DateTime DateTime { get; set; }

    /// <summary>
    /// Gets or sets the quantity.
    /// </summary>
    [Range(1, int.MaxValue, ErrorMessage = "Quantity must be a positive value.")]
    [MinLength(1, ErrorMessage = "Quantity must be at least 1 character long.")]
    public int Quantity { get; set; }

    /// <summary>
    /// Gets or sets the total cost.
    /// </summary>
    [MinLength(1, ErrorMessage = "TotalCost must be at least 1 character long.")]
    [Range(0, int.MaxValue, ErrorMessage = "TotalCost must be a non-negative value.")]

    public int TotalCost { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the operation is complete.
    /// </summary>
    public bool IsCompleted { get; set; } = false;

    /// <summary>
    /// Gets or sets the comment.
    /// </summary>
    [MaxLength(200, ErrorMessage = "Comment must be at most 200 characters long.")]
    public string? Comment { get; set; } = string.Empty;




}
