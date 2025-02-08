using System.ComponentModel.DataAnnotations;

namespace _04LibraryWeb.Models;

public class ConfirmViewModel
{
    public required string Username { get; set; }
    
    [Required]
    [Length(6, 6, ErrorMessage = "The token must be 6 characters.")]
    public string Token { get; set; }
}