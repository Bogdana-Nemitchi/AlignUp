using System;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Collections.Generic;
using AlignUp.Domain.Model.User;
using System.Data.Entity.Infrastructure;
namespace AlignUp.Domain.Core
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

   
    public class HttpCookie
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
        public HttpCookie(string name, string value)
        {
            Name = name;
            Value = value;
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

    public class UserApi : UserApiBase
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