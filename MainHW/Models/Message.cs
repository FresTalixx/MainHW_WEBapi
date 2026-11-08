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
    public int Id { get; set; } = _nextId++;
    [MinLength(3, ErrorMessage = "From must be at least 3 characters long.")]
    [EmailAddress(ErrorMessage = "Invalid email format.")]
    public string From { get; set; } = string.Empty;
    [MinLength(1, ErrorMessage = "Text must be at least 1 character long.")]
    public string Text { get; set; } = string.Empty;
    public DateTime DateTime { get; set; } = DateTime.Now;


}