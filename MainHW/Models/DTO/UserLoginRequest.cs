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
    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress(ErrorMessage = "Invalid email format.")]
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the password.
    /// </summary>
    [Required(ErrorMessage = "Password is required.")]
    public string Password { get; set; } = string.Empty;
}
