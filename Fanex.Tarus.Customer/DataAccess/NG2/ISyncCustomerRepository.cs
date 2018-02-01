using Fanex.CustomerSetting.Module.Core.Models;
using Fanex.Tarus.Customer.Entities.NG2;

namespace Fanex.Tarus.Customer.DataAccess.NG2
{
    public interface ISyncCustomerRepository
    {
        SyncCustomerModel GetCustomersByUsernames(string usernames);

        SyncCustomerModel GetCustomersByCustIds(string custIds);

        Result SyncCustomers(string custIds, bool isSync, int productId);
    }
}