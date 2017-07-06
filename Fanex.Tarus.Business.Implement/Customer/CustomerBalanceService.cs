using Fanex.Tarus.Business.Interface.Customer;
using Fanex.Tarus.DataAccess.Implement.Customer;
using Fanex.Tarus.DataAccess.Interface.Customer;
using Fanex.Tarus.Entities.Criteria;
using Fanex.Tarus.Web.Models.Customer.Balance;

namespace Fanex.Tarus.Business.Implement.Customer.Balance
{
    public class CustomerBalanceService: ICustomerBalanceService
    {
        private readonly ICustomerBalanceRepository _customerBalanceRepository;

        public CustomerBalanceService()
        {
            _customerBalanceRepository = new CustomerBalanceRepository();
        }

        public CustomerBalanceViewModel GetCustomerBalance(CustomerBalanceCriteria criteria)
        {
            return _customerBalanceRepository.GetCustomerBalance(criteria);
        }
    }
}
