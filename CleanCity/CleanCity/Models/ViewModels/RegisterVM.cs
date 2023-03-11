using System.ComponentModel.DataAnnotations;

namespace CleanCity.Models.ViewModels
{
    public class RegisterVM
    {
        [EmailAddress]
        [Required(ErrorMessage = "Email is required")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }
        [Required(ErrorMessage = "Nickname is required")]
        public string Nickname { get; set; }
    }
}