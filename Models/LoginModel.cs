using DocumentFormat.OpenXml.Presentation;
using System.ComponentModel.DataAnnotations;


namespace DocsService.Models
{
    public class LoginModel
    {
        [Required(ErrorMessage = "Логин обязателен")]
        public string Login { get; set; }

        [Required(ErrorMessage = "Пароль обязателен")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
