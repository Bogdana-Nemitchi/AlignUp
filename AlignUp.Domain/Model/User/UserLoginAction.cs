using AlignUp.Domain.Model.User;
using System;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using AlignUp.BusinessLogic.Helpers;

public UserLoginResponseDTO UserLoginAction(UserLoginDTO data)
{
    if (data == null || string.IsNullOrWhiteSpace(data.Username) || string.IsNullOrWhiteSpace(data.Password))
    {
        return new UserLoginResponseDTO
        {
            Status = false,
            StatusMessage = "Datele de autentificare sunt incomplete"
        };
    }

    string hashedPassword = PasswordHelper.HashPassword(data.Password.Trim());

    using (ApplicationDbContext db = new ApplicationDbContext())
    {
        UserDbTable user;
        bool isEmail = new EmailAddressAttribute().IsValid(data.Username.Trim());

        user = isEmail
            ? db.Users.FirstOrDefault(u => u.Email == data.Username.Trim() && u.PasswordHash == hashedPassword)
            : db.Users.FirstOrDefault(u => u.Username == data.Username.Trim() && u.PasswordHash == hashedPassword);

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
                LastLogin = (DateTime)user.LastLogin,
                LastIp = user.LastIp,
                UserRole = Enum.TryParse<UserRole>(user.UserRole, out var role) ? role : UserRole.Standard
            }
        };
    }
}
