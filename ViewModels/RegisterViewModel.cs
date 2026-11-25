using System.ComponentModel.DataAnnotations;

namespace StudentPortal.ViewModels;

public class RegisterViewModel
{
    [Required(ErrorMessage = "Email өрісін толтырыңыз.")]
    [EmailAddress(ErrorMessage = "Email мекенжайының пішімі дұрыс емес.")]
    [StringLength(254, ErrorMessage = "Email 254 таңбадан аспауы керек.")]
    [Display(Name = "Email")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Атыңызды енгізіңіз.")]
    [StringLength(60, MinimumLength = 2, ErrorMessage = "Аты-жөні 2–60 таңба аралығында болуы керек.")]
    [Display(Name = "Аты")]
    public string FirstName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Тегіңізді енгізіңіз.")]
    [StringLength(60, MinimumLength = 2, ErrorMessage = "Тегі 2–60 таңба аралығында болуы керек.")]
    [Display(Name = "Тегі")]
    public string LastName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Құпиясөзді енгізіңіз.")]
    [StringLength(100, MinimumLength = 8, ErrorMessage = "Құпиясөз кемінде 8 таңбадан тұруы керек.")]
    [DataType(DataType.Password)]
    [Display(Name = "Құпиясөз")]
    public string Password { get; set; } = string.Empty;

    [Required(ErrorMessage = "Құпиясөзді растаңыз.")]
    [DataType(DataType.Password)]
    [Compare("Password", ErrorMessage = "Құпиясөздер бірдей емес.")]
    [Display(Name = "Құпиясөзді растау")]
    public string ConfirmPassword { get; set; } = string.Empty;
}
