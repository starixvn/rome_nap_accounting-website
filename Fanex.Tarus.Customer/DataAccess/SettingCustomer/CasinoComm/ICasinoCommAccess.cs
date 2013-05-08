using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fanex.Tarus.Customer.Entities;

namespace Fanex.Tarus.Customer.DataAccess.SettingCustomer.CasinoComm
{
    public interface ICasinoCommAccess
    {
        Result UpdateCasinoComm(int custId, decimal valuleComm, int userId, string ip);
        int GetCustIdByUsername(string username);
        ResultCasinoComm CheckUsername(string username);
    }
}
