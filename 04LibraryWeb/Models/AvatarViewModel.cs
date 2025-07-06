using System.ComponentModel.DataAnnotations;

namespace _04LibraryWeb.Models;

public class AvatarViewModel
{
    public string Url { get; set; }
    
    [Display(Name = "Profile Avatar")]
    public IFormFile AvatarFile { get; set; }
}