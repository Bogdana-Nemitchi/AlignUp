namespace AlignUp.Domain.Model.User
{
    public class UserRegisterDTO
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }  // 🔧 Adaugă aceasta
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
