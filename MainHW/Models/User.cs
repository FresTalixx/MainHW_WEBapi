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
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string Password { get; set; } = string.Empty; // In a real application, passwords should be hashed and salted for security

    public string Token { get; set; } = string.Empty;
    
    public bool IsActive { get; set; }
    public int RoleId { get; set; } 

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

}
