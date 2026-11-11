using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace MainHW.Models;


public static class Roles
{
    public const int Admin = 1;
    public const int User = 2;
}

public class User
{
    /// <summary>
    /// Gets or sets the identifier.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the name.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the email address.
    /// </summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the password.
    /// </summary>
    /// <remarks>Store passwords securely using hashing and salting; do not retain plaintext values.</remarks>
    public string Password { get; set; } = string.Empty; // In a real application, passwords should be hashed and salted for security

    /// <summary>
    /// Gets or sets the token.
    /// </summary>
    public string Token { get; set; } = string.Empty;
    
    /// <summary>
    /// Indicates whether the object is active.
    /// </summary>
    public bool IsActive { get; set; }

    /// <summary>
    /// Gets or sets the role identifier.
    /// </summary>
    public int RoleId { get; set; } 

    /// <summary>
    /// Gets or sets the creation timestamp.
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

}
