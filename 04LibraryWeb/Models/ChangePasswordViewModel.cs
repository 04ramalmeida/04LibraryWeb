using System.ComponentModel.DataAnnotations;

namespace _04LibraryWeb.Models;

public class ChangePasswordViewModel
{
    
    [DataType(DataType.Password)]
    [MinLength(7, ErrorMessage = "Password must be at least 7 characters long.")]
    public required string CurrentPassword { get; set; }
    
    [DataType(DataType.Password)]
    [MinLength(7, ErrorMessage = "Password must be at least 7 characters long.")]
    public required string NewPassword { get; set; }
}