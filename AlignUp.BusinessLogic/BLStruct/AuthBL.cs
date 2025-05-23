using AlignUp.BusinessLogic.Core;
using AlignUp.BusinessLogic.Interface;
using AlignUp.Domain.Model.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlignUp.BusinessLogic.BLStruct
{
    public class AuthBL : UserApi, IAuth
    {
        public void AddUser(Domain.Model.User.UserDbTable user)
        {
            throw new NotImplementedException();
        }

        public bool ChangeUserRole(int userId, UserRole newRole)
        {
            throw new NotImplementedException();
        }

        public void DeleteUser(int id)
        {
            throw new NotImplementedException();
        }

        public List<Domain.Model.User.UserDbTable> GetAllUsers()
        {
            throw new NotImplementedException();
        }

        public List<Domain.Model.User.UserDbTable> GetRecentUsers(int count)
        {
            throw new NotImplementedException();
        }

        public int GetTotalUserCount()
        {
            throw new NotImplementedException();
        }

        public Domain.Model.User.UserDbTable GetUserById(int id)
        {
            throw new NotImplementedException();
        }

        public UserInfo GetUserInfoByToken(string token)
        {
            throw new NotImplementedException();
        }

        public List<Domain.Model.User.UserDbTable> GetUsersByRole(UserRole role)
        {
            throw new NotImplementedException();
        }

        public void UpdateUser(Domain.Model.User.UserDbTable user)
        {
            throw new NotImplementedException();
        }

        public string UserAuthLogic(Domain.Model.User.UserLoginDTO data)
        {
            return UserAuthLogicAction(data);
        }

        private string UserAuthLogicAction(Domain.Model.User.UserLoginDTO data)
        {
            throw new NotImplementedException();
        }

        public string UserAuthWithLogic(Domain.Model.User.UserLoginDTO loginDataForLogic)
        {
            throw new NotImplementedException();
        }
        public UserLoginResponseDTO UserLogin(Domain.Model.User.UserLoginDTO userLogin)
        {
            throw new NotImplementedException();
        }

        public bool UserRegister(UserRegisterDTO userRegister)
        {
            throw new NotImplementedException();
        }

        public bool ValidateUserToken(string token)
        {
            throw new NotImplementedException();
        }
    }
}
