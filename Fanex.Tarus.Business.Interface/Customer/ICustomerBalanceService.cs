using Fanex.Tarus.Entities.Criteria;
using Fanex.Tarus.Web.Models.Customer.Balance;
using System.Collections.Generic;

namespace Fanex.Tarus.Business.Interface.Customer
{
    public interface ICustomerBalanceService
    {
        CustomerBalanceViewModel GetCustomerBalance(CustomerBalanceCriteria criteria);
    }
}
