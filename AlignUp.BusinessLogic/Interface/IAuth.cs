using AlignUp.Domain.Model.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlignUp.BusinessLogic.Interface
{
    public interface IAuth
    {
        string UserAuthLogic(UserLoginDTO data);
    }
}
