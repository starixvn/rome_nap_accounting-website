using Fanex.Tarus.Entities.Criteria;
using Fanex.Tarus.Web.Models.Customer.Balance;

namespace Fanex.Tarus.DataAccess.Interface.Customer
{
    public interface ICustomerBalanceRepository
    {
        CustomerBalanceViewModel GetCustomerBalance(CustomerBalanceCriteria criteria);
    }
}
