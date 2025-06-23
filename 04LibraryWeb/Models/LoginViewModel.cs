
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace _04LibraryWeb.Models;

public class LoginViewModel
{

	[Required]
	[EmailAddress]
	public required string Email { get; set; }
	
	[Required]
	[PasswordPropertyText(true)]
	public required string Password { get; set; }
	
}
