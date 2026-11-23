using System.ComponentModel.DataAnnotations;

namespace MainHW.Models;

/// <summary>
/// Represents a message containing a sender, text, identifier, and date and time.
/// </summary>
public class Message
{
    //Створити проєкт веб - апі

    //Для повідомлень

    //повдомлення
    //- ід
    //- від кого (адреса)
    //- текст повідомлення
    //- дата час
    /// <summary>
    /// Gets or sets the unique identifier.
    /// </summary>
    public int Id { get; set; }
    
    /// <summary>
    /// Gets or sets the sender's email address.
    /// </summary>
    /// <remarks>The value must be at least three characters long and in a valid email address
    /// format.</remarks>
    public string From { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the text.
    /// </summary>
    public string Text { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the date and time.
    /// </summary>
    public DateTime DateTime { get; set; } = DateTime.Now;


}