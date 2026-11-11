namespace MainHW.Models;
/// <summary>
/// Represents an order.
/// </summary>
/// 

//Створіть модель замовлення з полями:
//номер замовлення;
//дата замовлення;
//кількість товарів;
//вартість товарів;
//дата-час замовлення;
//виконано(так/ні);
//коментар.

//Номер замовлення, дату-час замовлення і позначку «виконано» заповнює сервер.Нове замовлення завжди невиконане.
public class Order
{
    /// <summary>
    /// Gets or sets the order number.
    /// </summary>
    public int Number { get; set; }

    /// <summary>
    /// Gets or sets the date and time when the order was placed.
    /// </summary>
    public DateTime DateTime { get; set; }

    /// <summary>
    /// Gets or sets the quantity.
    /// </summary>
    public int Quantity { get; set; }

    /// <summary>
    /// Gets or sets the total cost.
    /// </summary>
    public int TotalCost { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the operation is complete.
    /// </summary>
    public bool IsCompleted { get; set; } = false;

    /// <summary>
    /// Gets or sets the comment.
    /// </summary>
    public string? Comment { get; set; } = string.Empty;




}
