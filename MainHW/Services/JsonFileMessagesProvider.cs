using MainHW.Models;

namespace MainHW.Services;

public class JsonFileMessagesProvider : JsonFileMessageProvider<Message>, IMessageProvider<Message>
{
    public JsonFileMessagesProvider(string dataFilePath) : base(dataFilePath)
    {
    }
}
