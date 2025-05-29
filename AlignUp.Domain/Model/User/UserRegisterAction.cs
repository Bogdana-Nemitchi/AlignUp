using AlignUp.Domain.Model.User;
using System;
using System.ComponentModel.DataAnnotations;
using AlignUp.BusinessLogic.Helpers;

public UserRegisterResponseDTO UserRegisterAction(UserRegisterDTO data)
{
    if (data == null || string.IsNullOrWhiteSpace(data.Username) ||
        string.IsNullOrWhiteSpace(data.Email) || string.IsNullOrWhiteSpace(data.Password))
    {
        return new UserRegisterResponseDTO
        {
            Status = false,
            StatusMessage = "Datele de înregistrare sunt incomplete"
        };
    }

    string username = data.Username.Trim();
    string email = data.Email.Trim();
    string password = data.Password.Trim();

    if (!new EmailAddressAttribute().IsValid(email))
    {
        return new UserRegisterResponseDTO
        {
            Status = false,
            StatusMessage = "Email invalid"
        };
    }

    if (password.Length < 8)
    {
        return new UserRegisterResponseDTO
        {
            Status = false,
            StatusMessage = "Parola prea scurtă"
        };
    }

    using (ApplicationDbContext db = new ApplicationDbContext())
    {
        if (db.Users.Any(u => u.Username == username))
        {
            return new UserRegisterResponseDTO
            {
                Status = false,
                StatusMessage = "Username deja folosit"
            };
        }

        if (db.Users.Any(u => u.Email == email))
        {
            return new UserRegisterResponseDTO
            {
                Status = false,
                StatusMessage = "Email deja folosit"
            };
        }

        string hashed = PasswordHelper.HashPassword(password);

        UserDbTable user = new UserDbTable
        {
            Username = username,
            Email = email,
            PasswordHash = hashed,
            RegistrationDateTime = DateTime.Now,
            RegistrationIp = data.RegistrationIp,
            LastLogin = DateTime.Now,
            LastIp = data.RegistrationIp,
            UserRole = UserRole.Standard.ToString()
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
