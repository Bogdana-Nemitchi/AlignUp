using System;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using static AlignUp.Domain.Core.UserApi;

namespace AlignUp.Domain.Core
{
    public class UserApiBase
    {
        /// <summary>
        /// Creează un cookie pentru autentificarea utilizatorului
        /// </summary>
        public HttpCookie CreateAuthCookie(string username)
        {
            try
            {
                string token = Guid.NewGuid().ToString();

                var cookie = new HttpCookie("auth_token", token)
                {
                    Expires = DateTime.Now.AddDays(7),
                    HttpOnly = true
                };

                using (var db = new ApplicationDbContext())
                {
                    var existingSession = db.Sessions.FirstOrDefault(s => s.Username == username);

                    if (existingSession != null)
                    {
                        existingSession.Token = token;
                        existingSession.ExpiryDate = DateTime.Now.AddDays(7);
                        db.Entry(existingSession).State = EntityState.Modified;
                    }
                    else
                    {
                        db.Sessions.Add(new Session
                        {
                            Username = username,
                            Token = token,
                            CreatedDate = DateTime.Now,
                            ExpiryDate = DateTime.Now.AddDays(7)
                        });
                    }

                    db.SaveChanges();
                }

                return cookie;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Eroare la crearea cookie-ului: {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// Autentifică utilizatorul pe baza datelor de login
        /// </summary>
        public UserLoginResponseDTO UserLoginAction(UserLoginDTO data)
        {
            try
            {
                if (data == null || string.IsNullOrEmpty(data.Username) || string.IsNullOrEmpty(data.Password))
                {
                    return new UserLoginResponseDTO
                    {
                        Status = false,
                        StatusMessage = "Datele de autentificare sunt incomplete"
                    };
                }

                UserDbTable result;
                var validate = new EmailAddressAttribute();
                var pass = PasswordHelper.HashPassword(data.Password);

                using (var db = new ApplicationDbContext())
                {
                    if (validate.IsValid(data.Username))
                    {
                        result = db.Users.FirstOrDefault(u => u.Email == data.Username && u.Password == pass);
                    }
                    else
                    {
                        result = db.Users.FirstOrDefault(u => u.Username == data.Username && u.Password == pass);
                    }

                    if (result == null)
                    {
                        return new UserLoginResponseDTO
                        {
                            Status = false,
                            StatusMessage = "Username sau parolă incorectă"
                        };
                    }

                    result.LastIp = data.UserIp;
                    result.LastLogin = DateTime.Now;
                    db.Entry(result).State = EntityState.Modified;
                    db.SaveChanges();

                    return new UserLoginResponseDTO
                    {
                        Status = true,
                        StatusMessage = "Autentificare reușită",
                        UserInfo = new UserInfo
                        {
                            Id = result.Id,
                            Username = result.Username,
                            Email = result.Email,
                            LastLogin = result.LastLogin ?? DateTime.Now,
                            LastIp = result.LastIp,
                            UserRole = result.UserRole ?? UserApi.UserRole.Standard
                        }
                    };
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Eroare la autentificare: {ex.Message}");
                return new UserLoginResponseDTO
                {
                    Status = false,
                    StatusMessage = "A apărut o eroare în timpul autentificării"
                };
            }
        }

        /// <summary>
        /// Înregistrează un utilizator nou
        /// </summary>
        public UserRegisterResponseDTO UserRegisterAction(UserRegisterDTO data)
        {
            try
            {
                if (data == null ||
                    string.IsNullOrEmpty(data.Username) ||
                    string.IsNullOrEmpty(data.Email) ||
                    string.IsNullOrEmpty(data.Password))
                {
                    return new UserRegisterResponseDTO
                    {
                        Status = false,
                        StatusMessage = "Datele de înregistrare sunt incomplete"
                    };
                }

                var emailValidator = new EmailAddressAttribute();
                if (!emailValidator.IsValid(data.Email))
                {
                    return new UserRegisterResponseDTO
                    {
                        Status = false,
                        StatusMessage = "Adresa de email nu este validă"
                    };
                }

                if (data.Password.Length < 8)
                {
                    return new UserRegisterResponseDTO
                    {
                        Status = false,
                        StatusMessage = "Parola trebuie să conțină minim 8 caractere"
                    };
                }

                using (var db = new ApplicationDbContext())
                {
                    if (db.Users.Any(u => u.Username == data.Username))
                    {
                        return new UserRegisterResponseDTO
                        {
                            Status = false,
                            StatusMessage = "Numele de utilizator este deja folosit"
                        };
                    }

                    if (db.Users.Any(u => u.Email == data.Email))
                    {
                        return new UserRegisterResponseDTO
                        {
                            Status = false,
                            StatusMessage = "Adresa de email este deja folosită"
                        };
                    }

                    var hashedPassword = PasswordHelper.HashPassword(data.Password);

                    var newUser = new UserDbTable
                    {
                        Username = data.Username,
                        Email = data.Email,
                        Password = hashedPassword,
                        RegistrationDateTime = DateTime.Now,
                        RegistrationIp = data.RegistrationIp,
                        LastLogin = DateTime.Now,
                        LastIp = data.RegistrationIp,
                        UserRole = UserRole.Standard
                    };

                    db.Users.Add(newUser);
                    db.SaveChanges();

                    return new UserRegisterResponseDTO
                    {
                        Status = true,
                        StatusMessage = "Înregistrare reușită"
                    };
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Eroare la înregistrare: {ex.Message}");
                return new UserRegisterResponseDTO
                {
                    Status = false,
                    StatusMessage = "A apărut o eroare în timpul înregistrării"
                };
            }
        }

        /// <summary>
        /// Validează un token de autentificare
        /// </summary>
        public UserInfo ValidateAuthToken(string token)
        {
            try
            {
                if (string.IsNullOrEmpty(token))
                {
                    return null;
                }

                using (var db = new ApplicationDbContext())
                {
                    var session = db.Sessions.FirstOrDefault(s =>
                        s.Token == token && s.ExpiryDate > DateTime.Now);

                    if (session == null) return null;

                    var user = db.Users.FirstOrDefault(u =>
                        u.Username == session.Username || u.Email == session.Username);

                    if (user == null) return null;

                    return new UserInfo
                    {
                        Id = user.Id,
                        Username = user.Username,
                        Email = user.Email,
                        LastLogin = user.LastLogin ?? DateTime.Now,
                        LastIp = user.LastIp,
                        UserRole = user.UserRole ?? UserRole.Standard
                    };
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Eroare la validarea token-ului: {ex.Message}");
                return null;
            }
        }
    }
}
