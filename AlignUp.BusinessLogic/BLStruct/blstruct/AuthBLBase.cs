using AlignUp.Domain.Core;
using System;
using UserDbTable = AlignUp.Domain.Core.UserDbTable;
using UserDbTableCore = AlignUp.Domain.Core.UserDbTable;

namespace AlignUp.Domain.BLStruct
{
    public class AuthBLBase
    {

        public void AddUser(UserDbTable user)
        {
            var internalUser = new UserDbTableCore
            {
                Username = user.Username,
                Email = user.Email,
                Password = user.Password,
                RegistrationIp = user.RegistrationIp,
                LastLogin = user.LastLogin,
                LastIp = user.LastIp,
                UserRole = (UserApi.UserRole?)user.UserRole
            };

            AddUser(internalUser);
        }

        public void AddUserTransformed(UserDbTable user)
        {
            var internalUser = new UserDbTable
            {
                Username = user.Username,
                Email = user.Email,
                Password = user.Password,
                RegistrationIp = user.RegistrationIp,
                LastLogin = user.LastLogin,
                LastIp = user.LastIp,
                UserRole = (UserApi.UserRole?)user.UserRole
            };

            AddUserToDb(internalUser);
        }

        public void AddUserToDb(UserDbTable user)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                user.Password = PasswordHelper.HashPassword(user.Password);
                user.RegistrationDateTime = DateTime.Now;

                db.Users.Add(user);
                db.SaveChanges();
                System.Diagnostics.Debug.WriteLine($"✔ Utilizator adăugat: {user.Username}");
            }
        }


        public string UserAuthLogic(Domain.Model.User.UserLoginDTO loginDataForLogic)
        {
            throw new NotImplementedException();
        }

        public bool UserRegister(Domain.Model.User.UserRegisterDTO userRegister)
        {
            throw new NotImplementedException();
        }
    }
}