using AlignUp.BusinessLogic.BLStruct;
using AlignUp.BusinessLogic.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlignUp.BusinessLogic
{
    public class BusinessLogic
    {
        public IAuth GetAuthBL()
        {
            return new AuthBL();
        }
    }
}
