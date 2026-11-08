using MainHW.Models;

namespace MainHW.Services;

public class JsonFileMessagesProvider : JsonFileMessageProvider<Message>, IDataProvider<Message>
{
    public JsonFileMessagesProvider(string dataFilePath) : base(dataFilePath)
    {
    }
}
