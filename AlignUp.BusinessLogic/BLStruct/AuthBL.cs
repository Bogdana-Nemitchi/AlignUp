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
        public void AddUser(UserDbTable user) { }

        public bool ChangeUserRole(int userId, UserApi.UserRole newRole) => true;

        public void DeleteUser(int id) { }

        public List<UserDbTable> GetAllUsers() => new List<UserDbTable>();

        public List<UserDbTable> GetRecentUsers(int count) => new List<UserDbTable>();

        public int GetTotalUserCount() => 0;

        public UserDbTable GetUserById(int id) => new UserDbTable();

        public UserInfo GetUserInfoByToken(string token) => new UserInfo();

        public List<UserDbTable> GetUsersByRole(UserApi.UserRole role) => new List<UserDbTable>();

        public void UpdateUser(UserDbTable user) { }

        public string UserAuthLogic(UserLoginDTO data) => data.Username == "admin" && data.Password == "admin" ? Guid.NewGuid().ToString() : null;

        public UserLoginResponseDTO UserLogin(UserLoginDTO userLogin) => new UserLoginResponseDTO();

        public bool UserRegister(UserRegisterDTO userRegister) => true;

        public bool ValidateUserToken(string token) => true;
    }
}
