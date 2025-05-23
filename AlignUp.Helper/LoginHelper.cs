using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;


namespace eUseControl.Helpers
{
    public static class LoginHelper
    {

        /// <param name="password">Parola în text clar</param>
        public static string HashGen(string password)
        {
            if (string.IsNullOrEmpty(password))
                return string.Empty;

            using (var sha256 = SHA256.Create())
            {
                var bytes = Encoding.UTF8.GetBytes(password);
                var hash = sha256.ComputeHash(bytes);
                return Convert.ToBase64String(hash);
            }
        }

       
        /// <param name="password">Parola furnizată de utilizator</param>
        /// <param name="storedHash">Hash-ul stocat în baza de date</param>
        public static bool VerifyPassword(string password, string storedHash)
        {
            if (string.IsNullOrEmpty(password) || string.IsNullOrEmpty(storedHash))
                return false;

            var passwordHash = HashGen(password);
            return passwordHash == storedHash;
        }

        public static string GenerateResetToken()
        {
            return Guid.NewGuid().ToString("N");
        }
    }
}