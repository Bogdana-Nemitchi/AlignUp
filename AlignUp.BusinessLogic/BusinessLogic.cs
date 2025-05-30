using AlignUp.Domain.BLStruct;
using AlignUp.Domain.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlignUp.Domain
{
    public class BusinessLogic
    {
        public IAuth GetAuthBL()
        {
            return new AuthBL();
        }
    }
}
