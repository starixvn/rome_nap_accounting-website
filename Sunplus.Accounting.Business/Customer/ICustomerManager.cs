using System.Collections;
using System.Collections.Generic;
using Sunplus.Accounting.Entities;
using Sunplus.Accounting.Entities.Casino;
using Sunplus.Accounting.Entities.Customer;

namespace Sunplus.Accounting.Business.Customer
{
    public interface ICustomerManager
    {
        CustDetail GetCustomerInfo(string username);

        Hashtable GenDownlineList(int custId, int roleId, int pageIndex);

        string GenOnlineList(int custId);

        CustomerDetail GetCustomerDetail(string username);

        CustomerDetail GetCustomerStatus(int custId);

        Hashtable GetHtmlCustInfoContent(string userName);

        Result UpdateCustomerSite(int custId, string newSite);

        Result UpdateDangerousCustomer(int custId, int updateDownline, int dangerType, int dangerLevel);

        Sunplus.Accounting.Entities.Customer.CustomerExRate SelectCustomerExRate(string username);

        Result CustomerCrossTransferInsert(int fromCustID, int toCustID, double fromAmount, double toAmount, double fromExRate, double toExRate, string forexUsername, string remark, int custid);

        List<Sunplus.Accounting.Entities.Customer.ForexAccount> SelectAllForexAccount();

        BetSettingNG CheckEnableNGDownline(string username);

        Result UpdateBetSettingNGDownline(string username, int isEnable, string minbet, string maxbet, string maxMatch, string MaxBall);

        Result AutoInsertThridPartyAccount(CSCustomerDetail custInfo, CustDetail uplineInfo, int userId, string userName, string ip);

        string GetRNGSiteNameByCustId(int custId);
    }
}