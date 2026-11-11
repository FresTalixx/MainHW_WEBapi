using System.ComponentModel.DataAnnotations;

namespace MainHW.Models;

public class Message
{
    //Створити проєкт веб - апі

    //Для повідомлень

    //повдомлення
    //- ід
    //- від кого (адреса)
    //- текст повідомлення
    //- дата час
    private static int _nextId = 1;
    /// <summary>
    /// Gets or sets the unique identifier.
    /// </summary>
    public int Id { get; set; } = _nextId++;
    
    /// <summary>
    /// Gets or sets the sender's email address.
    /// </summary>
    /// <remarks>The value must be at least three characters long and in a valid email address
    /// format.</remarks>
    [MinLength(3, ErrorMessage = "From must be at least 3 characters long.")]
    [EmailAddress(ErrorMessage = "Invalid email format.")]
    
    public string From { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the text.
    /// </summary>
    [MinLength(1, ErrorMessage = "Text must be at least 1 character long.")]
    public string Text { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the date and time.
    /// </summary>
    public DateTime DateTime { get; set; } = DateTime.Now;


}