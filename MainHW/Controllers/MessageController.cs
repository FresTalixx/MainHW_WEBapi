using Microsoft.AspNetCore.Mvc;
using MainHW.Models;
using MainHW.Services;
using MainHW.Models.DTO;
using AutoMapper;

namespace MainHW.Controllers;

[ApiController]
[Route("messages")]
public class MessageController: ControllerBase
{
    private readonly MessageService messageService;
    private readonly IMapper _mapper;

    public MessageController(MessageService messageService, IMapper mapper)
    {
        this.messageService = messageService;
        this._mapper = mapper;
    }

    /// <summary>
    /// Gets all messages.
    /// </summary>
    /// <returns>A result containing the collection of messages.</returns>
    [HttpGet]
    public async Task<ActionResult<ICollection<Message>>> GetAllMessagesAsync()
    {
        var messages = await messageService.GetAllMessagesAsync();
        return Ok(messages);
    }

    // GET /messages/search?minPrice=10&maxPrice=100
    /// <summary>
    /// Searches for messages matching the specified criteria.
    /// </summary>
    /// <param name="searchMessage">The search criteria, including the sender and date filters.</param>
    /// <returns>An action result containing the matching messages, or a bad request response if the sender is not specified or
    /// the date is in the future.</returns>
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
    /// <summary>
    /// Retrieves a message by its identifier.
    /// </summary>
    /// <param name="id">The identifier of the message to retrieve.</param>
    /// <returns>An action result containing the message if found; otherwise, a 404 Not Found response.</returns>
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

    /// <summary>
    /// Adds a message for the authenticated user.
    /// </summary>
    /// <param name="request">The message to add. The sender email must match the email associated with the authorization token.</param>
    /// <returns>An action result containing the added message, or an error response if the user is unauthenticated or the sender
    /// email does not match.</returns>
    [HttpPost]
    public async Task<ActionResult<Message>> AddMessageAsync([FromBody] MessageAddRequest request)
    {
        var user = HttpContext.Items["User"] as User;
        if (user == null)
        {
            return Unauthorized("User is not authenticated.");
        }

        if (!string.Equals(user.Email, request.From, StringComparison.OrdinalIgnoreCase))
        {
            return StatusCode(StatusCodes.Status403Forbidden, "The sender email must match the email associated with the authorization token.");
        }
        
        var newMessage = _mapper.Map<Message>(request);
        newMessage.DateTime = DateTime.Now; // Set the current date and time
        var message = await messageService.AddMessageAsync(newMessage);
        return Ok(message);
    }
        
    /// <summary>
    /// Updates a message.
    /// </summary>
    /// <param name="id">The identifier of the message to update.</param>
    /// <param name="updatedMessage">The updated message.</param>
    /// <returns>An action result containing the updated message, or NotFound if the message does not exist.</returns>
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

    /// <summary>
    /// Deletes a message.
    /// </summary>
    /// <param name="id">The unique identifier of the message to delete.</param>
    /// <returns>An <see cref="OkResult"/> if the message is deleted; otherwise, an <see cref="NotFoundResult"/>.</returns>
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
