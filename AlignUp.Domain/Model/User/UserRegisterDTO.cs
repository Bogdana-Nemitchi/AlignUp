namespace AlignUp.Domain.Model.User
{
    public class UserRegisterDTO
    {
        public string Username { get; set; }
        public string Email { get; set; }

        public string Password { get; set; }
        public string ConfirmPassword { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string RegistrationIp { get; set; } 
    }
}
