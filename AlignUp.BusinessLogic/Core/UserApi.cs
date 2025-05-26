using AlignUp.BusinessLogic.DBModel;
using AlignUp.Domain.Model.User;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;

namespace AlignUp.BusinessLogic.Core
{
    public class UserApi
    {
        public UserLoginResponseDTO UserLoginAction(UserLoginDTO data)
        {
            if (data == null || string.IsNullOrEmpty(data.Username) || string.IsNullOrEmpty(data.Password))
            {
                return new UserLoginResponseDTO
                {
                    Status = false,
                    StatusMessage = "Datele de autentificare sunt incomplete"
                };
            }

            string hashedPassword = PasswordHelper.HashPassword(data.Password);
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                UserDbTable user;
                bool isEmail = new EmailAddressAttribute().IsValid(data.Username);

                user = isEmail
                    ? db.Users.FirstOrDefault(u => u.Email == data.Username && u.Password == hashedPassword)
                    : db.Users.FirstOrDefault(u => u.Username == data.Username && u.Password == hashedPassword);

                if (user == null)
                {
                    return new UserLoginResponseDTO
                    {
                        Status = false,
                        StatusMessage = "Username sau parolă incorectă"
                    };
                }

                user.LastLogin = DateTime.Now;
                user.LastIp = data.UserIp;
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();

                return new UserLoginResponseDTO
                {
                    Status = true,
                    StatusMessage = "Autentificare reușită",
                    UserInfo = new UserInfo
                    {
                        Id = user.Id,
                        Username = user.Username,
                        Email = user.Email,
                        LastLogin = user.LastLogin ?? DateTime.Now,
                        LastIp = user.LastIp,
                        UserRole = (UserRole)user.UserRole
                    }
                };
            }
        }

        public UserRegisterResponseDTO UserRegisterAction(UserRegisterDTO data)
        {
            if (data == null || string.IsNullOrEmpty(data.Username) || string.IsNullOrEmpty(data.Email) || string.IsNullOrEmpty(data.Password))
            {
                return new UserRegisterResponseDTO
                {
                    Status = false,
                    StatusMessage = "Datele de înregistrare sunt incomplete"
                };
            }

            if (!new EmailAddressAttribute().IsValid(data.Email))
            {
                return new UserRegisterResponseDTO
                {
                    Status = false,
                    StatusMessage = "Email invalid"
                };
            }

            if (data.Password.Length < 8)
            {
                return new UserRegisterResponseDTO
                {
                    Status = false,
                    StatusMessage = "Parola prea scurtă"
                };
            }

            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                if (db.Users.Any(u => u.Username == data.Username))
                {
                    return new UserRegisterResponseDTO
                    {
                        Status = false,
                        StatusMessage = "Username deja folosit"
                    };
                }

                if (db.Users.Any(u => u.Email == data.Email))
                {
                    return new UserRegisterResponseDTO
                    {
                        Status = false,
                        StatusMessage = "Email deja folosit"
                    };
                }

                string hashed = PasswordHelper.HashPassword(data.Password);

                UserDbTable user = new UserDbTable
                {
                    Username = data.Username,
                    Email = data.Email,
                    Password = hashed,
                    RegistrationDateTime = DateTime.Now,
                    RegistrationIp = data.RegistrationIp,
                    LastLogin = DateTime.Now,
                    LastIp = data.RegistrationIp,
                    UserRole = (int)UserRole.Standard
                };

                db.Users.Add(user);
                db.SaveChanges();

                return new UserRegisterResponseDTO
                {
                    Status = true,
                    StatusMessage = "Înregistrare completă"
                };
            }
        }

        // alte metode rămân neschimbate...
    }

    public static class PasswordHelper
    {
        public static string HashPassword(string password)
        {
            if (string.IsNullOrWhiteSpace(password)) return string.Empty;

            using (var sha256 = System.Security.Cryptography.SHA256.Create())
            {
                byte[] bytes = System.Text.Encoding.UTF8.GetBytes(password);
                byte[] hash = sha256.ComputeHash(bytes);
                return Convert.ToBase64String(hash);
            }
        }
    }
}
