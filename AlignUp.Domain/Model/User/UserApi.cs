using System;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Collections.Generic;
using AlignUp.Domain.Model.User;
using System.Data.Entity.Infrastructure;
namespace AlignUp.BusinessLogic.Core
{
  
    public class UserDbTable
    {
      
        public int Id { get; set; }

        /// <summary>
        /// Numele de utilizator
        /// </summary>
        [Required]
        [MaxLength(50)]
        public string Username { get; set; }

        /// <summary>
        /// Email-ul utilizatorului
        /// </summary>
        [Required]
        [EmailAddress]
        [MaxLength(100)]
        public string Email { get; set; }

     
        [Required]
        public string Password { get; set; }

        
        public DateTime RegistrationDateTime { get; set; }

       
        public string RegistrationIp { get; set; }

        
        public DateTime? LastLogin { get; set; }

        
        public string LastIp { get; set; }

     
        public UserApi.UserRole? UserRole { get; set; }
    }

   
    internal class HttpCookie
    {
       
        public string Name { get; set; }

        /// <summary>
        /// Valoarea cookie-ului
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// Data expirării cookie-ului
        /// </summary>
        public DateTime Expires { get; set; }

        /// <summary>
        /// Proprietate care indică dacă cookie-ul este accesibil doar serverului
        /// </summary>
        public bool HttpOnly { get; set; }

        /// <summary>
        /// Constructor pentru cookie cu nume
        /// </summary>
        public HttpCookie(string name)
        {
            Name = name;
        }
    }

    /// <summary>
    /// DTO pentru cererea de login
    /// </summary>
    public class UserLoginDTO
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string UserIp { get; set; }
    }

    /// <summary>
    /// DTO pentru răspunsul de login
    /// </summary>
    public class UserLoginResponseDTO
    {
        public bool Status { get; set; }
        public string StatusMessage { get; set; }
        public UserInfo UserInfo { get; set; }
    }

    /// <summary>
    /// DTO pentru informațiile utilizatorului
    /// </summary>
    public class UserInfo
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public DateTime LastLogin { get; set; }
        public string LastIp { get; set; }
        public UserApi.UserRole UserRole { get; set; }
    }

    public class UserApi
    {
        /// <summary>
        /// Definește tipurile de roluri pentru utilizatori
        /// </summary>
        public enum UserRole
        {
            /// <summary>
            /// Utilizator standard
            /// </summary>
            Standard = 0,

            /// <summary>
            /// Administrator cu drepturi depline
            /// </summary>
            Admin = 1
        }

        internal UserLoginResponseDTO UserLoginAction(UserLoginDTO data)
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
                    // Verificăm dacă credențialul este un email
                    if (validate.IsValid(data.Username))
                    {
                        result = db.Users.FirstOrDefault(u => u.Email == data.Username && u.Password == pass);
                    }
                    else
                    {
                        // Credențialul este un username
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

                    // Actualizăm informațiile de login
                    result.LastIp = data.UserIp;
                    result.LastLogin = DateTime.Now;
                    db.Entry(result).State = EntityState.Modified;
                    db.SaveChanges();

                    // Returnăm informațiile utilizatorului
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
                            UserRole = result.UserRole ?? UserRole.Standard
                        }
                    };
                }
            }
            catch (Exception ex)
            {
                // Logăm excepția
                System.Diagnostics.Debug.WriteLine($"Eroare la autentificare: {ex.Message}");
                return new UserLoginResponseDTO
                {
                    Status = false,
                    StatusMessage = "A apărut o eroare în timpul autentificării"
                };
            }
        }

        /// <summary>
        /// DTO pentru cererea de înregistrare
        /// </summary>
        public class UserRegisterDTO
        {
            public string Username { get; set; }
            public string Email { get; set; }
            public string Password { get; set; }
            public string RegistrationIp { get; set; }
        }

        /// <summary>
        /// DTO pentru răspunsul la înregistrare
        /// </summary>
        public class UserRegisterResponseDTO
        {
            public bool Status { get; set; }
            public string StatusMessage { get; set; }
        }

        /// <summary>
        /// Acțiunea de înregistrare a unui utilizator nou
        /// </summary>
        internal UserRegisterResponseDTO UserRegisterAction(UserRegisterDTO data)
        {
            try
            {
                // Validăm datele
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

                // Validăm adresa de email
                var emailValidator = new EmailAddressAttribute();
                if (!emailValidator.IsValid(data.Email))
                {
                    return new UserRegisterResponseDTO
                    {
                        Status = false,
                        StatusMessage = "Adresa de email nu este validă"
                    };
                }

                // Validăm parola (minim 8 caractere)
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
                    // Verificăm dacă username-ul sau email-ul există deja
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

                    // Hash pentru parolă
                    var hashedPassword = PasswordHelper.HashPassword(data.Password);

                    // Creăm utilizatorul nou
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

                    // Salvăm în baza de date
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
                // Logăm excepția
                System.Diagnostics.Debug.WriteLine($"Eroare la înregistrare: {ex.Message}");
                return new UserRegisterResponseDTO
                {
                    Status = false,
                    StatusMessage = "A apărut o eroare în timpul înregistrării"
                };
            }
        }

        /// <summary>
        /// Creează un cookie pentru autentificarea utilizatorului
        /// </summary>
        internal HttpCookie CreateAuthCookie(string username)
        {
            try
            {
                // Generăm token-ul pentru cookie
                string token = Guid.NewGuid().ToString();

                var cookie = new HttpCookie("auth_token")
                {
                    Value = token,
                    Expires = DateTime.Now.AddDays(7), // Expiră după 7 zile
                    HttpOnly = true
                };

                using (var db = new ApplicationDbContext())
                {
                    // Verificăm dacă există deja o sesiune pentru utilizator
                    var existingSession = db.Sessions.FirstOrDefault(s => s.Username == username);

                    if (existingSession != null)
                    {
                        // Actualizăm sesiunea existentă
                        existingSession.Token = token;
                        existingSession.ExpiryDate = DateTime.Now.AddDays(7);
                        db.Entry(existingSession).State = EntityState.Modified;
                    }
                    else
                    {
                        // Creăm o sesiune nouă
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
        /// Validează un token de autentificare și returnează informațiile utilizatorului
        /// </summary>
        internal UserInfo ValidateAuthToken(string token)
        {
            try
            {
                if (string.IsNullOrEmpty(token))
                {
                    return null;
                }

                using (var db = new ApplicationDbContext())
                {
                    // Găsim sesiunea
                    var session = db.Sessions.FirstOrDefault(s =>
                        s.Token == token && s.ExpiryDate > DateTime.Now);

                    if (session == null)
                    {
                        return null;
                    }

                    // Găsim utilizatorul
                    var user = db.Users.FirstOrDefault(u =>
                        u.Username == session.Username || u.Email == session.Username);

                    if (user == null)
                    {
                        return null;
                    }

                    // Returnăm informațiile utilizatorului
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

    /// <summary>
    /// Helper pentru operațiuni cu parole
    /// </summary>
    public static class PasswordHelper
    {
        /// <summary>
        /// Generează un hash pentru o parolă
        /// </summary>
        public static string HashPassword(string password)
        {
            if (string.IsNullOrEmpty(password))
                return string.Empty;

            using (var sha256 = System.Security.Cryptography.SHA256.Create())
            {
                var bytes = System.Text.Encoding.UTF8.GetBytes(password);
                var hash = sha256.ComputeHash(bytes);
                return Convert.ToBase64String(hash);
            }
        }
    }

    /// <summary>
    /// Modelul pentru sesiunea utilizatorului
    /// </summary>
    public class Session
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Token { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ExpiryDate { get; set; }
    }

    /// <summary>
    /// Contextul bazei de date pentru aplicație
    /// </summary>
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext() : base("DefaultConnection")
        {
        }

        public DbSet<UserDbTable> Users { get; set; }
        public DbSet<Session> Sessions { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configurare pentru entitatea UserDbTable
            modelBuilder.Entity<UserDbTable>()
                .ToTable("Users")
                .HasKey(u => u.Id);

            // Configurare pentru entitatea Session
            modelBuilder.Entity<Session>()
                .ToTable("Sessions")
                .HasKey(s => s.Id);
        }
    }
}