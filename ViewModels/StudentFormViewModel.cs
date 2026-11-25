using System.ComponentModel.DataAnnotations;

namespace StudentPortal.ViewModels;

public class StudentFormViewModel
{
    [Required(ErrorMessage = "Студенттің аты-жөнін енгізіңіз.")]
    [StringLength(120, MinimumLength = 2, ErrorMessage = "Аты-жөні 2–120 таңба аралығында болуы керек.")]
    [Display(Name = "Аты-жөні")]
    public string FullName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Email өрісін толтырыңыз.")]
    [EmailAddress(ErrorMessage = "Email мекенжайының пішімі дұрыс емес.")]
    [StringLength(254)]
    [Display(Name = "Email")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Тобын енгізіңіз.")]
    [StringLength(30, ErrorMessage = "Топ 30 таңбадан аспауы керек.")]
    [Display(Name = "Тобы")]
    public string Group { get; set; } = string.Empty;

    [DataType(DataType.Date)]
    [Display(Name = "Қабылданған күні")]
    public DateTime EnrollmentDate { get; set; } = DateTime.Today;
}
