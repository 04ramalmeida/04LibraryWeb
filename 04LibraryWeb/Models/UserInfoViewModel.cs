using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace _04LibraryWeb.Models;

public class UserInfoViewModel
{
    public required string FirstName { get; set; }
    
    public required string LastName { get; set; }

    public string? Username { get; set; }
    public string? Email { get; set; }
    

    [DataType(DataType.DateTime)]
    public DateTime? CreatedOn { get; set; }
}