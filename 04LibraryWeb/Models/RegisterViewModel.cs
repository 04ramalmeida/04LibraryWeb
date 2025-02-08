using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace _04LibraryWeb.Models;

public class RegisterViewModel
{
    [EmailAddress]
    public required string Username { get; set; }
    
    [PasswordPropertyText(true)]
    [MinLength(7, ErrorMessage = "Password must be at least 7 characters long.")]
    public required string Password { get; set; }
    
    [Length(2, 50, ErrorMessage = "First Name must be between 2 and 50 characters.")]
    public required string FirstName { get; set; }
    
    [Length(2, 50, ErrorMessage = "Last Name must be between 2 and 50 characters.")]
    public required string LastName { get; set; }
}