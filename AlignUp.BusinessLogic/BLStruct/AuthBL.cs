using System;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using AlignUp.Domain.Model.User;
using AlignUp.BusinessLogic.DBModel;
using AlignUp.BusinessLogic.Interface;

namespace AlignUp.BusinessLogic.BLStruct
{
    public class AuthBL : AuthBLBase, IAuth
    {
        public string Username { get; private set; }
        public int Id { get; private set; }
        public string Email { get; private set; }

        public AuthBL() { }

        private string UserAuthLogicAction(UserLoginDTO data)
        {
            if (data == null)
            {
                return null;
            }

            try
            {
                using (ApplicationDbContext db = new ApplicationDbContext())
                {
                    UserDbTable user = db.Users.FirstOrDefault(u => u.Username == data.Username);
                    if (user == null) return null;

                    string computedHash = HashPassword(data.Password);
                    if (!user.PasswordHash.Equals(computedHash, StringComparison.OrdinalIgnoreCase)) return null;
                    if (!user.IsActive) return null;

                    string token = Convert.ToBase64String(Guid.NewGuid().ToByteArray());

                    RefreshToken refreshToken = new RefreshToken
                    {
                        UserId = user.Id,
                        Token = token,
                        CreatedAt = DateTime.UtcNow,
                        ExpiresAt = DateTime.UtcNow.AddDays(data.RememberMe ? 30 : 1),
                        IsRevoked = false
                    };

                    db.RefreshTokens.Add(refreshToken);
                    user.LastLoginAt = DateTime.UtcNow;
                    db.SaveChanges();

                    return token;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Eroare la autentificare: " + ex.Message);
                return null;
            }
        }

        public string UserAuthWithLogic(UserLoginDTO data) => UserAuthLogicAction(data);

        public bool UserRegister(UserRegisterDTO data)
        {
            if (data == null)
                return false;

            try
            {
                using (ApplicationDbContext db = new ApplicationDbContext())
                {
                    bool userExists = db.Users.Any(u => u.Username == data.Username || u.Email == data.Email);
                    if (userExists) return false;

                    string passwordHash = HashPassword(data.Password);

                    UserDbTable newUser = new UserDbTable
                    {
                        Username = data.Username,
                        Email = data.Email,
                        PasswordHash = passwordHash,
                        Role = 0,
                        IsActive = true,
                        CreatedAt = DateTime.UtcNow,
                        EmailConfirmed = false,
                        FirstName = data.FirstName,
                        LastName = data.LastName
                    };

                    db.Users.Add(newUser);
                    db.SaveChanges();

                    return true;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Eroare la înregistrare: " + ex.Message);
                return false;
            }
        }

        public bool ValidateUserToken(string token)
        {
            try
            {
                using (ApplicationDbContext db = new ApplicationDbContext())
                {
                    RefreshToken refreshToken = db.RefreshTokens
                        .FirstOrDefault(r => r.Token == token && r.ExpiresAt > DateTime.UtcNow && !r.IsRevoked);

                    return refreshToken != null;
                }
            }
            catch
            {
                return false;
            }
        }

        private string HashPassword(string password)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(password));
                return BitConverter.ToString(bytes).Replace("-", "").ToLowerInvariant();
            }
        }

        // Implementări pentru interfață
        UserLoginResponseDTO IAuth.UserLogin(UserLoginDTO data)
        {
            string token = UserAuthWithLogic(data);
            return new UserLoginResponseDTO
            {
                Status = !string.IsNullOrEmpty(token),
                StatusMessage = string.IsNullOrEmpty(token) ? "Autentificare eșuată" : "Autentificare reușită",
                TokenCreated = token,
                Username = data.Username
            };
        }

        string IAuth.UserAuthWithLogic(UserLoginDTO data) => UserAuthWithLogic(data);
        bool IAuth.UserRegister(UserRegisterDTO data) => UserRegister(data);
        bool IAuth.ValidateUserToken(string token) => ValidateUserToken(token);
        string IAuth.UserAuthLogic(UserLoginDTO data) => UserAuthWithLogic(data);
    }

    public class AuthBLBase { }
}
