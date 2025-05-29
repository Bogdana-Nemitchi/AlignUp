using AlignUp.BusinessLogic.Core;
using AlignUp.BusinessLogic.Interface;
using AlignUp.Domain.Model.User;
using System;
using System.Collections.Generic;
using static AlignUp.BusinessLogic.Core.UserApi;

namespace AlignUp.BusinessLogic.BLStruct
{
    public class AuthBL : IAuth
    {
        public void AddUser(Domain.Model.User.UserDbTable user)
        {
            throw new NotImplementedException();
        }

        public void AddUser(Core.UserDbTable user)
        {
            throw new NotImplementedException();
        }

        public bool ChangeUserRole(int userId, UserApi.UserRole newRole) => true;

        public void DeleteUser(int id) { }

        public List<Domain.Model.User.UserDbTable> GetAllUsers()
        {
            throw new NotImplementedException();
        }

        public List<Domain.Model.User.UserDbTable> GetRecentUsers(int count)
        {
            throw new NotImplementedException();
        }

        public int GetTotalUserCount() => 0;

        public Domain.Model.User.UserDbTable GetUserById(int id)
        {
            throw new NotImplementedException();
        }

        public UserInfo GetUserInfoByToken(string token) => new UserInfo();

        public List<Domain.Model.User.UserDbTable> GetUsersByRole(UserRole role)
        {
            throw new NotImplementedException();
        }

        public void UpdateUser(Domain.Model.User.UserDbTable user)
        {
            throw new NotImplementedException();
        }

        public string UserAuthLogic(Domain.Model.User.UserLoginDTO loginDataForLogic)
        {
            throw new NotImplementedException();
        }

        public UserLoginResponseDTO UserLogin(Domain.Model.User.UserLoginDTO userLogin)
        {
            throw new NotImplementedException();
        }

        public bool UserRegister(UserRegisterDTO userRegister) => true;

        public bool ValidateUserToken(string token) => true;
    }
}
