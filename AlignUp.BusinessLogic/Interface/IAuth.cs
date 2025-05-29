using System;
using System.Collections.Generic;
using AlignUp.BusinessLogic.Core;
using AlignUp.Domain.Model.User;

namespace AlignUp.BusinessLogic.Interface
{
    public interface IAuth
    {
        // Metode pentru autentificare utilizator
        UserLoginResponseDTO UserLogin(Domain.Model.User.UserLoginDTO userLogin);

        // Metode pentru înregistrare utilizator
        bool UserRegister(UserApi.UserRegisterDTO userRegister);

        // Metode pentru gestionarea sesiunilor
        bool ValidateUserToken(string token);
        UserInfo GetUserInfoByToken(string token);

        // Metode pentru administrare utilizatori

        List<Domain.Model.User.UserDbTable> GetAllUsers();
        Domain.Model.User.UserDbTable GetUserById(int id);
        void AddUser(Domain.Model.User.UserDbTable user);
        void UpdateUser(Domain.Model.User.UserDbTable user);
        void DeleteUser(int id);

        // Metode pentru gestionarea rolurilor utilizatorilor
        List<Domain.Model.User.UserDbTable> GetUsersByRole(UserApi.UserRole role);
        bool ChangeUserRole(int userId, UserApi.UserRole newRole);

        // Metode pentru statistici
        int GetTotalUserCount();

        List<Domain.Model.User.UserDbTable> GetRecentUsers(int count);

        string UserAuthLogic(Domain.Model.User.UserLoginDTO loginDataForLogic);
        void AddUser(Core.UserDbTable user);
    }
}