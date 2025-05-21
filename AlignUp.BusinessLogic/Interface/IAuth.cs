using System;
using System.Collections.Generic;
using AlignUp.Domain.Model.User;

namespace AlignUp.BusinessLogic.Interface
{
    public interface IAuth
    {
        // Metode pentru autentificare utilizator
        UserLoginResponseDTO UserLogin(UserLoginDTO userLogin);

        // Metode pentru înregistrare utilizator
        bool UserRegister(UserRegisterDTO userRegister);

        // Metode pentru gestionarea sesiunilor
        bool ValidateUserToken(string token);
        UserInfo GetUserInfoByToken(string token);

        // Metode pentru administrare utilizatori
        List<UserDbTable> GetAllUsers();
        UserDbTable GetUserById(int id);
        void AddUser(UserDbTable user);
        void UpdateUser(UserDbTable user);
        void DeleteUser(int id);

        // Metode pentru gestionarea rolurilor utilizatorilor
        List<UserDbTable> GetUsersByRole(UserRole role);
        bool ChangeUserRole(int userId, UserRole newRole);

        // Metode pentru statistici
        int GetTotalUserCount();
        List<UserDbTable> GetRecentUsers(int count);
    }
}