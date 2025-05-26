using System.ComponentModel.DataAnnotations;

namespace AlignUp.Web.Models.Auth
{
    public class UserDataLogin
    {
        [Required(ErrorMessage = "Numele de utilizator este obligatoriu")]
        [Display(Name = "Nume de utilizator")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Parola este obligatorie")]
        [DataType(DataType.Password)]
        [Display(Name = "Parolă")]
        public string Password { get; set; }

        [Display(Name = "Ține-mă minte")]
        public bool RememberMe { get; set; }
    }
}