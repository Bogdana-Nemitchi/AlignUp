using System;

namespace AlignUp.BusinessLogic.Core
{
    public class UserInfo
    {
        public bool IsAuthenticated { get; set; }
        public int UserId { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public DateTime? LastLoginTime { get; set; }
        public DateTime RegistrationDate { get; set; }
    }
}
