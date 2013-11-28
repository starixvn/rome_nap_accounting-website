using System.Collections.Generic;
using Fanex.Tarus.Finance.Entities;
using Fanex.Tarus.Finance.Model;
using Fanex.Tarus.Finance.Model.Customer;

namespace Fanex.Tarus.Finance.Business.Customer
{
    public interface IFinanceCustomerListService
    {
        CustomerList GetSuperList(List<PostParam> postParams);

        CustomerList GetMasterList(List<PostParam> postParams);

        CustomerList GetAgentList(List<PostParam> postParams);

        CustomerList GetMemberList(List<PostParam> postParams);

        List<Exchange> GetCurrencyList();
    }
}