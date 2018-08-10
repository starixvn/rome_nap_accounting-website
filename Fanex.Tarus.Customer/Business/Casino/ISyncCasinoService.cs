using Fanex.CustomerSetting.Module.Core.Models;
using Fanex.Tarus.Customer.Entities.NG2;
using System.Collections.Generic;

namespace Fanex.Tarus.Customer.Business.Casino
{
    public interface ISyncCasinoService
    {
        SyncCustomerModel GetCustomersByUsernames(string usernames);

        SyncCustomerModel GetUnsyncedCustomers();

        SyncCustomerModel SyncCustomers(SyncCustomerModel model);
    }
}