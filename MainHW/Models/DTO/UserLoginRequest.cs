using System.ComponentModel.DataAnnotations;

namespace MainHW.Models.DTO;

/// <summary>
/// 
/// </summary>
public class UserLoginRequest
{

    /// <summary>
    /// Gets or sets the email address.
    /// </summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the password.
    /// </summary>
    public string Password { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the background file.
    /// </summary>
    public IFormFile? BgFile { get; set; } 
}
