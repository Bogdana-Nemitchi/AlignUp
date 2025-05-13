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
        public string UserAuthLogic(UserLoginDTO data)
        {
            return UserAuthLogicAction(data);
        }
    }
}
