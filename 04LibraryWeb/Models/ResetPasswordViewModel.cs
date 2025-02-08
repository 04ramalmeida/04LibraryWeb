using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace _04LibraryWeb.Models;

public class ResetPasswordViewModel
{
    [EmailAddress]
    public required string Email { get; set; }
    
    [Length(6,6, ErrorMessage = "The token must be 6 characters long.")]
    public required string Token { get; set; }
    
    [DataType(DataType.Password)]
    [MinLength(7, ErrorMessage = "The password must be at least 7 characters long.")]
    public required string Password { get; set; }
    
}