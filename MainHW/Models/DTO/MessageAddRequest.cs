namespace MainHW.Models.DTO;

/// <summary>
/// Represents a request to add a message.
/// </summary>
public class MessageAddRequest
{
    /// <summary>
    /// Gets or sets the text.
    /// </summary>
    public string Text { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the source.
    /// </summary>
    public string From { get; set; } = string.Empty;
}
