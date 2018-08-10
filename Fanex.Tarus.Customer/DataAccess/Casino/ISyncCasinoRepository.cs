using Fanex.CustomerSetting.Module.Core.Models;
using Fanex.Tarus.Customer.Entities.NG2;
using System.Collections.Generic;

namespace Fanex.Tarus.Customer.DataAccess.Casino
{
    public interface ISyncCasinoRepository
    {
        SyncCustomerModel GetCustomersByUsernames(string usernames);

        SyncCustomerModel GetUnsyncedCustomers();

        IDictionary<string, Result> SyncCustomers(IList<SyncCustomerEntity> customers);
    }
}