using System.ComponentModel.DataAnnotations;

namespace StudentPortal.Models;

public class Student
{
    public int Id { get; set; }

    [Required]
    [StringLength(120)]
    public string FullName { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    [StringLength(254)]
    public string Email { get; set; } = string.Empty;

    [Required]
    [StringLength(30)]
    public string Group { get; set; } = string.Empty;

    [DataType(DataType.Date)]
    public DateTime EnrollmentDate { get; set; } = DateTime.Today;
}
