namespace AlignUp.Domain.Model.User
{
    public class UserLoginDTO
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string UserIp { get; set; }
        public bool RememberMe { get; set; }  // NECESAR pentru AuthBL
    }
}
