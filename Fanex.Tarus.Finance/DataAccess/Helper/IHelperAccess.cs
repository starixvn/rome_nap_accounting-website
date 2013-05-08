using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace Fanex.Tarus.Finance.DataAccess.Helper
{
    public interface IHelperAccess
    {

        DataTable GetForexAccounts();

        DataTable BalanceDateInfo();

        bool CheckIsInternal(string userName);
    }
}
