namespace _04LibraryWeb.Models;

public class UserInfoViewModel
{
    public required string FirstName { get; set; }
    
    public required string LastName { get; set; }

    public required string Email { get; set; }
    
    public required DateTime CreatedOn { get; set; }
}