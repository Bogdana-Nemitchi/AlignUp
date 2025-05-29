using System;
using System.Security.Cryptography;
using System.Text;

namespace AlignUp.BusinessLogic.Helpers
{
    public static class PasswordHelper
    {
        public static string HashPassword(string password)
        {
            if (string.IsNullOrWhiteSpace(password)) return string.Empty;

            using (var sha256 = SHA256.Create())
            {
                byte[] bytes = Encoding.UTF8.GetBytes(password);
                byte[] hash = sha256.ComputeHash(bytes);
                return Convert.ToBase64String(hash);
            }
        }
    }
}
