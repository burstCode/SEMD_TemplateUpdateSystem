using System.ComponentModel.DataAnnotations;

namespace TemplatesWebsite.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Данное поле не может быть пустым")]
        [EmailAddress(ErrorMessage = "Введите корректный адрес электронной почты")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Данное поле не может быть пустым")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Запомнить вход?")]
        public bool RememberMe { get; set; }
    }
}
