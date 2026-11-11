using System.ComponentModel.DataAnnotations;

namespace MainHW.Models.DTO;

/// <summary>
/// Represents the data required to register a user.
/// </summary>
/// <remarks>The name must be between 3 and 100 characters, the email address must be valid, and the password must
/// contain at least 6 characters.</remarks>
public class UserRegisterRequest
{
    /// <summary>
    /// Gets or sets the name.
    /// </summary>
    [Required(ErrorMessage = "Name is required.")]
    [MinLength(3, ErrorMessage = "Name must be at least 3 characters long.")]
    [MaxLength(100, ErrorMessage = "Name cannot exceed 100 characters.")]
    //[StringLength(100, ErrorMessage = "Name cannot exceed 100 characters.")]
    public string Name { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the email address.
    /// </summary>
    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress(ErrorMessage = "Invalid email format.")]
    public string Email { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the password.
    /// </summary>
    /// <remarks>The value must contain at least six characters. Store passwords securely using appropriate
    /// hashing and salting.</remarks>
    [Required(ErrorMessage = "Password is required.")]
    [MinLength(6, ErrorMessage = "Password must be at least 6 characters long.")]
    public string Password { get; set; } = string.Empty; // In a real application, passwords should be hashed and salted for security
}
