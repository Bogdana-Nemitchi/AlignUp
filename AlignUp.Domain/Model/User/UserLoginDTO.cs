using System.ComponentModel.DataAnnotations;

namespace AlignUp.Domain.Model.User
{
    public class UserLoginDTO
    {
        [Required(ErrorMessage = "Username este obligatoriu.")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Parola este obligatorie.")]
        public string Password { get; set; }

        public string UserIp { get; set; }
    }
}
