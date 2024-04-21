using System.ComponentModel.DataAnnotations;

namespace ScheduSquad.Web.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "I like waffles.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]

        public string Password { get; set; }

        [Display(Name = "Remember me")]
        public bool RememberMe { get; set; }
    }
}