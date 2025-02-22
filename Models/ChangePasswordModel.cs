using System.ComponentModel.DataAnnotations;

public class ChangePasswordModel
{
    [Required]
    [DataType(DataType.Password)]
    [Display(Name = "Eski Şifre")]
    public string OldPassword { get; set; }

    [Required]
    [DataType(DataType.Password)]
    [Display(Name = "Yeni Şifre")]
    public string NewPassword { get; set; }

    [Required]
    [DataType(DataType.Password)]
    [Display(Name = "Yeni Şifre (Tekrar)")]
    [Compare("NewPassword", ErrorMessage = "Yeni şifreler eşleşmiyor.")]
    public string ConfirmNewPassword { get; set; }
}