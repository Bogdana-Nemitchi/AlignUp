using System;

namespace AlignUp.Domain.Model.User
{
    public class UserInfo
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public DateTime LastLogin { get; set; }
        public string LastIp { get; set; }
        public UserRole UserRole { get; set; }
    }
}
