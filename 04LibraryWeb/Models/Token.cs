using System;

namespace _04LibraryWeb.Models;

public class Token
{
	public required string AccessToken { get; set; }
	
	public required string TokenType { get; set; }
	
	public required string UserId { get; set; }
	
	public required string UserName { get; set; }
}
