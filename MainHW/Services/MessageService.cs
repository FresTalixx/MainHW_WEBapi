using MainHW.Models;

namespace MainHW.Services;

public class MessageService
{
    private readonly IMessageProvider<Message> _messageProvider;

    public MessageService(IMessageProvider<Message> messageProvider)
    {
        this._messageProvider = messageProvider;
    }

    public async Task<ICollection<Message>> GetAllMessagesAsync()
    {
        return await _messageProvider.GetAllAsync();
    }

    public async Task<ICollection<Message>> SearchMessagesAsync(SearchMessage searchMessage)
    {
        var products = await _messageProvider.GetAllAsync();
        return products.Where(p => p.From == searchMessage.From && p.DateTime > searchMessage.DateTime).ToList();
    }



    public async Task<Message?> GetMessageByIdAsync(int id)
    {
        var products = await _messageProvider.GetAllAsync();
        return products.FirstOrDefault(p => p.Id == id);
    }

    public async Task<Message> AddMessageAsync(Message newMessage)
    {
        var messages = await _messageProvider.GetAllAsync();
        newMessage.Id = messages.Any() ? messages.Max(p => p.Id) + 1 : 1;
        messages.Add(newMessage);
        await _messageProvider.SaveAsync(messages);
        return newMessage;
    }

    public async Task<Message?> UpdateMessageAsync(int id, Message updatedMessage)
    {
        var messages = await _messageProvider.GetAllAsync();
        var message = messages.FirstOrDefault(p => p.Id == id);
        if (message == null)
        {
            return null;
        }
        message.DateTime = updatedMessage.DateTime;
        message.Text  = updatedMessage.Text;
        message.From =  updatedMessage.From;
        
        await _messageProvider.SaveAsync(messages);
        return message;
    }

    public async Task<bool> DeleteMessageAsync(int id)
    {
        var products = await _messageProvider.GetAllAsync();
        var product = products.FirstOrDefault(p => p.Id == id);
        if (product == null)
        {
            return false;
        }
        products.Remove(product);
        await _messageProvider.SaveAsync(products);
        return true;
    }
}
