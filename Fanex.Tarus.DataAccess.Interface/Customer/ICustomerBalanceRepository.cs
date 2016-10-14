using Fanex.Tarus.Entities.Criteria;
using Fanex.Tarus.Web.Models.Customer.Balance;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fanex.Tarus.DataAccess.Interface.Customer
{
    public interface ICustomerBalanceRepository
    {
        CustomerBalanceViewModel GetCustomerBalance(CustomerBalanceCriteria criteria);
    }
}
