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
    public string From { get; set; } = string.Empty;
    public string Text { get; set; } = string.Empty;
    public DateTime DateTime { get; set; } = DateTime.Now;


}