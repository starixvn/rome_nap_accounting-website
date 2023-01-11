namespace Sunplus.Accounting.Business.Customer
{
    using Entities;
    using Entities.Customer;
    using System.Collections;
    using System.Collections.Generic;

    public interface ICustomerManager
    {
        CustDetail GetCustomerInfo(string username);

        CustDetail GetCustomerInfoMoveSite(string username);

        Hashtable GenDownlineList(int custId, int roleId, int pageIndex);

        string GenOnlineList(int custId);

        CustomerDetail GetCustomerDetail(string username);

        CustomerDetail GetCustomerStatus(int custId);

        Hashtable GetHtmlCustInfoContent(string userName, byte checkCustId = 0);

        Result UpdateCustomerSite(int custId, string newSite);

        CustomerExRate SelectCustomerExRate(string username);

        Result CustomerCrossTransferInsert(
            int fromCustID,
            int toCustID,
            double fromAmount,
            double toAmount,
            double fromExRate,
            double toExRate,
            string forexUsername,
            string remark,
            int custid);

        List<ForexAccount> SelectAllForexAccount();
    }
}