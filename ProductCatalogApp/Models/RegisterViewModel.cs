using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace ProductCatalogApp.Models
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Введите имя пользователя")]
        [Display(Name = "Имя пользователя")]
        public string Username { get; set; } = string.Empty;

        [Required(ErrorMessage = "Введите пароль")]
        [MinLength(6, ErrorMessage = "Пароль должен быть минимум 6 символов")]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "Повторите пароль")]
        [DataType(DataType.Password)]
        [Display(Name = "Подтверждение пароля")]
        [Compare(nameof(Password), ErrorMessage = "Пароли не совпадают")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}
