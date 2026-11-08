using Microsoft.AspNetCore.Mvc;
using MainHW.Models;
using MainHW.Services;

namespace MainHW.Controllers;

[ApiController]
[Route("messages")]
public class MessageController: ControllerBase
{
    private readonly MessageService messageService;

    public MessageController(MessageService messageService)
    {
        this.messageService = messageService;
    }

    [HttpGet]
    public async Task<ActionResult<ICollection<Message>>> GetAllMessagesAsync()
    {
        var messages = await messageService.GetAllMessagesAsync();
        return Ok(messages);
    }

    // GET /messages/search?minPrice=10&maxPrice=100
    [HttpGet("search")]
    public async Task<ActionResult<ICollection<Message>>> SearchMessagesAsync([FromQuery] SearchMessage searchMessage)
    {
        if (searchMessage.From == "")
        {
            return BadRequest("From must be specified.");
        }
        if (searchMessage.DateTime > DateTime.Now)
        {
            return BadRequest("Date can't be in the future.");
        }

        var messages = await messageService.SearchMessagesAsync(searchMessage);
        return Ok(messages);
    }


    // GET /messages/{id}
    [HttpGet("{id:int}")]
    public async Task<ActionResult<Message>> GetMessageByIdAsync(int id)
    {
        var message = await messageService.GetMessageByIdAsync(id);
        if (message == null)
        {
            return NotFound(); // 404 Not Found
        }
        return Ok(message);
    }

    [HttpPost]
    public async Task<ActionResult<Message>> AddMessageAsync([FromBody] Message newMessage)
    {
        var message = await messageService.AddMessageAsync(newMessage);
        return Ok(message);
    }
        
    [HttpPut("{id:int}")]
    public async Task<ActionResult<Message>> UpdateMessageAsync(int id, [FromBody] Message updatedMessage)
    {
        var message = await messageService.UpdateMessageAsync(id, updatedMessage);
        if (message == null)
        {
            return NotFound();
        }
        return Ok(message);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteMessageAsync(int id)
    {
        var result = await messageService.DeleteMessageAsync(id);
        if (!result)
        {
            return NotFound();
        }
        return Ok();
    }
}
