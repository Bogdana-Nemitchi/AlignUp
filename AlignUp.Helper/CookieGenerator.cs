using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;


namespace eUseControl.Helpers
{

    public static class CookieGenerator
    {
        
        /// <param name="credential">Username sau email-ul utilizatorului</param>
        public static string Create(string credential)
        {
            if (string.IsNullOrEmpty(credential))
                return string.Empty;

            var combinedString = credential + DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss");

            using (var sha256 = SHA256.Create())
            {
                var bytes = Encoding.UTF8.GetBytes(combinedString);
                var hash = sha256.ComputeHash(bytes);

                return Convert.ToBase64String(hash);
            }
        }

       
        /// <param name="cookieValue">Valoarea cookie-ului</param>
        /// <param name="storedValue">Valoarea stocată în baza de date</param>
        public static bool Validate(string cookieValue, string storedValue)
        {
            if (string.IsNullOrEmpty(cookieValue) || string.IsNullOrEmpty(storedValue))
                return false;

            return cookieValue == storedValue;
        }
    }
}