using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace StudentPortal.Models;

public class ApplicationUser : IdentityUser
{
    [Required]
    [StringLength(60)]
    public string FirstName { get; set; } = string.Empty;

    [Required]
    [StringLength(60)]
    public string LastName { get; set; } = string.Empty;
}
