namespace MainHW.Models;

/// <summary>
/// Represents a search message.
/// </summary>
public class SearchMessage
{
    /// <summary>
    /// Gets or sets the message sender.
    /// </summary>
    public string From { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the date and time.
    /// </summary>
    public DateTime DateTime { get; set; }
}
