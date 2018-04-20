using Fanex.Tarus.Customer.Entities.NG2;

namespace Fanex.Tarus.Customer.Business.NG2
{
    public interface ISyncCustomerService
    {
        SyncCustomerModel GetCustomersByUsernames(string usernames);

        SyncCustomerModel SyncCustomers(SyncCustomerModel model);
    }
}