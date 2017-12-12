namespace Sunplus.Accounting.Business.Customer
{
    using System.Collections;
    using System.Collections.Generic;
    using Entities;
    using Entities.Casino;
    using Entities.Customer;

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

        Result UpdateDangerousCustomer(int custId, int updateDownline, int dangerType, int dangerLevel);

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

        BetSettingNG CheckEnableNGDownline(string username);

        Result UpdateBetSettingNGDownline(
            string username, 
            int isEnable, 
            string minbet, 
            string maxbet, 
            string maxMatch, 
            string MaxBall);

        //Result AutoInsertThridPartyAccount(CSCustomerDetail custInfo, CustDetail uplineInfo, int userId, string userName, string ip);

        string GetRNGSiteNameByCustId(int custId);
    }
}