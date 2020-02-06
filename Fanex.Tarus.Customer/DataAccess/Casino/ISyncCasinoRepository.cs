namespace Fanex.Tarus.Customer.DataAccess.Casino
{
    using Fanex.CustomerSetting.Module.Core.Models;
    using Fanex.Tarus.Customer.Criteria.Casino;
    using Fanex.Tarus.Customer.Entities.NG2;
    using System.Collections.Generic;

    public interface ISyncCasinoRepository
    {
        SyncCustomerModel GetCustomersByUsernames(string usernames);

        SyncCustomerModel GetUnsyncedCustomers(GetUnsyncedListCriteria criteria);

        IDictionary<string, Result> SyncCustomers(IList<SyncCustomerEntity> customers);
    }
}