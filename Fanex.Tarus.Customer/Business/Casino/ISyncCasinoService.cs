namespace Fanex.Tarus.Customer.Business.Casino
{
    using Fanex.Tarus.Customer.Criteria.Casino;
    using Fanex.Tarus.Customer.Entities.NG2;

    public interface ISyncCasinoService
    {
        SyncCustomerModel GetCustomersByUsernames(string usernames);

        SyncCustomerModel GetUnsyncedCustomers(GetUnsyncedListCriteria criteria);

        SyncCustomerModel SyncCustomers(SyncCustomerModel model);
    }
}