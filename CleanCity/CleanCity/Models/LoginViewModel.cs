using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace CleanCity.Controllers {
    public class LoginViewModel {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        public string Password { get; set; }
    }
}