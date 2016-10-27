using Fanex.Tarus.Business.Interface.Customer;
using Fanex.Tarus.DataAccess.Implement.Customer;
using Fanex.Tarus.DataAccess.Interface.Customer;
using Fanex.Tarus.Entities.Criteria;
using Fanex.Tarus.Web.Models.Customer.Balance;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fanex.Tarus.Business.Implement.Customer.Balance
{
    public class CustomerBalanceService: ICustomerBalanceService
    {
        private ICustomerBalanceRepository _customerBalanceRepository;

        public CustomerBalanceService()
        {
            this._customerBalanceRepository = new CustomerBalanceRepository();
        }

        public CustomerBalanceViewModel GetCustomerBalance(CustomerBalanceCriteria criteria)
        {
            return this._customerBalanceRepository.GetCustomerBalance(criteria);
        }
    }
}
