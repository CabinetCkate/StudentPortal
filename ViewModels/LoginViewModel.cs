using System.ComponentModel.DataAnnotations;

namespace StudentPortal.ViewModels;

public class LoginViewModel
{
    [Required(ErrorMessage = "Email өрісін толтырыңыз.")]
    [EmailAddress(ErrorMessage = "Email мекенжайының пішімі дұрыс емес.")]
    [Display(Name = "Email")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Құпиясөзді енгізіңіз.")]
    [DataType(DataType.Password)]
    [Display(Name = "Құпиясөз")]
    public string Password { get; set; } = string.Empty;

    [Display(Name = "Мені есте сақтау")]
    public bool RememberMe { get; set; }
}
