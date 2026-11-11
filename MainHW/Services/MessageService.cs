using MainHW.Models;
using AutoMapper;

namespace MainHW.Services;

public class MessageService
{
    private readonly MessageRepository _messageRepository;
    private readonly IMapper _mapper;

    public MessageService(MessageRepository messageRepository, IMapper mapper)
    {
        this._messageRepository = messageRepository;
        this._mapper = mapper;
    }

    public async Task<ICollection<Message>> GetAllMessagesAsync()
    {
        return await _messageRepository.GetAllAsync();
    }

    public async Task<ICollection<Message>> SearchMessagesAsync(SearchMessage searchMessage)
    {
        var products = await _messageRepository.GetAllAsync();
        return products.Where(p => p.From == searchMessage.From && p.DateTime > searchMessage.DateTime).ToList();
    }

    public async Task<Message?> GetMessageByIdAsync(int id)
    {
        var products = await _messageRepository.GetAllAsync();
        return products.FirstOrDefault(p => p.Id == id);
    }

    public async Task<Message> AddMessageAsync(Message newMessage)
    {
        var messages = await _messageRepository.GetAllAsync();
        newMessage.Id = messages.Any() ? messages.Max(p => p.Id) + 1 : 1;
        messages.Add(newMessage);
        await _messageRepository.SaveChangesAsync();
        return newMessage;
    }

    public async Task<Message?> UpdateMessageAsync(int id, Message updatedMessage)
    {
        var messages = await _messageRepository.GetAllAsync();
        var message = messages.FirstOrDefault(p => p.Id == id);
        if (message == null)
        {
            return null;
        }
        message = _mapper.Map(updatedMessage, message);

        await _messageRepository.SaveChangesAsync();
        return message;
    }

    public async Task<bool> DeleteMessageAsync(int id)
    {
        var products = await _messageRepository.GetAllAsync();
        var product = products.FirstOrDefault(p => p.Id == id);
        if (product == null)
        {
            return false;
        }
        products.Remove(product);
        await _messageRepository.SaveChangesAsync();
        return true;
    }
}
