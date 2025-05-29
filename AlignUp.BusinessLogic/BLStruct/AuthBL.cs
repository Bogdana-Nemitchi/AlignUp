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
        private readonly UserApi _userApi = new UserApi();

        public string UserAuthLogic(Domain.Model.User.UserLoginDTO loginDataForLogic)
        {
            var response = _userApi.UserLoginAction(new Core.UserLoginDTO
            {
                Username = loginDataForLogic.Username,
                Password = loginDataForLogic.Password,
                UserIp = loginDataForLogic.UserIp
            });

            if (response.Status)
            {
                var cookie = _userApi.CreateAuthCookie(loginDataForLogic.Username);
                return cookie?.Value; // returnăm token-ul
            }

            return null; // autentificare eșuată
        }

        public UserLoginResponseDTO UserLogin(Domain.Model.User.UserLoginDTO userLogin)
        {
            return _userApi.UserLoginAction(new Core.UserLoginDTO
            {
                Username = userLogin.Username,
                Password = userLogin.Password,
                UserIp = userLogin.UserIp
            });
        }

        // TODO: Implement other IAuth methods as needed
        public void AddUser(Domain.Model.User.UserDbTable user) => throw new NotImplementedException();
        public void AddUser(Core.UserDbTable user) => throw new NotImplementedException();
        public bool ChangeUserRole(int userId, UserApi.UserRole newRole) => true;
        public void DeleteUser(int id) { }
        public List<Domain.Model.User.UserDbTable> GetAllUsers() => throw new NotImplementedException();
        public List<Domain.Model.User.UserDbTable> GetRecentUsers(int count) => throw new NotImplementedException();
        public int GetTotalUserCount() => 0;
        public Domain.Model.User.UserDbTable GetUserById(int id) => throw new NotImplementedException();
        public UserInfo GetUserInfoByToken(string token) => new UserInfo();
        public List<Domain.Model.User.UserDbTable> GetUsersByRole(UserRole role) => throw new NotImplementedException();
        public void UpdateUser(Domain.Model.User.UserDbTable user) => throw new NotImplementedException();
        public bool UserRegister(UserRegisterDTO userRegister) => true;
        public bool ValidateUserToken(string token) => true;
    }
}
