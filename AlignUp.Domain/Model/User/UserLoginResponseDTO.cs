namespace AlignUp.Domain.Model.User
{
    public class UserLoginResponseDTO
    {
        public bool Status { get; set; }
        public string StatusMessage { get; set; }
        public string TokenCreated { get; set; }   // folosit în AuthBL
        public string Username { get; set; }       // folosit în AuthBL
        public UserInfo UserInfo { get; set; }
    }
}
