using System;
using System.ServiceModel.Activation;

using Sunplus.Accounting.Business.BootStrappers;
using Sunplus.Accounting.Core.Common;
using Sunplus.Accounting.Entities.Customer;
using Sunplus.Accounting.Business.Customer;
using Sunplus.Core.LoggingLib;
using Sunplus.Accounting.Entities;
using System.Collections.Generic;

// NOTE: If you change the class name "ServiceCustomerInfo" here, you must also update the reference to "ServiceCustomerInfo" in Web.config.
namespace Sunplus.Accounting.WcfServiceLibrary.Customer
{
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class ServiceCustomerInfo : IServiceCustomerInfo
    {
        public string GenDownlineList(int custId, int roleId, int pageIndex)
        {
            var customerService = new Sunplus.Accounting.Business.ServiceCustomerInfo();
            return customerService.GenDownlineList( custId,  roleId,  pageIndex);
        }

        public string GenOnlineList(int custId)
        {
            var customerService = new Sunplus.Accounting.Business.ServiceCustomerInfo();
            return customerService.GenOnlineList(custId);
        }

        public CustDetail GetCustomerInfo(string username)
        {
            var customer = new CustDetail();
            try
            {
                var customerManager = CommonBootStrapper.Locator.GetInstance<ICustomerManager>();
                customer = customerManager.GetCustomerInfo(username);
            }
            catch (Exception ex)
            {
                
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
                
            }
           
            return customer;
        }


        public Result UpdateCustomerSite(int custId, string newSite)
        {

            var result = new Result();
            try
            {
                var customerManager = CommonBootStrapper.Locator.GetInstance<ICustomerManager>();
                result = customerManager.UpdateCustomerSite(custId, newSite);
            }
            catch (Exception ex)
            {
                
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);

            }
            return result;
        }

        public Result UpdateDangerousCustomerSer(int custId, int updateDownline, int dangerType, int dangerLevel)
        {
            var result = new Result();
            var customerManager = CommonBootStrapper.Locator.GetInstance<ICustomerManager>();
            result = customerManager.UpdateDangerousCustomer(custId, updateDownline, dangerType, dangerLevel);
            return result;
        }

        public Sunplus.Accounting.Entities.Customer.CustomerExRate SelectCustomerExRate(string username)
        {
            var customerManager = CommonBootStrapper.Locator.GetInstance<ICustomerManager>();
            return customerManager.SelectCustomerExRate(username);
        }

        public Result CustomerCrossTransferInsert(int fromCustID, int toCustID, double fromAmount, double toAmount, double fromExRate, double toExRate, string forexUsername, string remark, int custid)
        {
            var customerManager = CommonBootStrapper.Locator.GetInstance<ICustomerManager>();
            return customerManager.CustomerCrossTransferInsert(fromCustID, toCustID, fromAmount, toAmount, fromExRate, toExRate, forexUsername, remark, AppSession.AccId);
        }

        public List<Sunplus.Accounting.Entities.Customer.ForexAccount> SelectAllForexAccount()
        {
            var customerManager = CommonBootStrapper.Locator.GetInstance<ICustomerManager>();
            return customerManager.SelectAllForexAccount();
        }

        public Result UpdBetSettingEnableNGCustomer(string username, int isEnable, string maxbet, string minbet, string maxperball, string maxpermatch)
        {
            var enableNG = CommonBootStrapper.Locator.GetInstance<ICustomerManager>();
            return enableNG.UpdateBetSettingNGDownline(username, isEnable, maxbet, minbet, maxpermatch, maxperball);
        }


        public BetSettingNG SelBetSettingUpline(string username)
        {
            var enableNG = CommonBootStrapper.Locator.GetInstance<ICustomerManager>();
            return enableNG.CheckEnableNGDownline(username);
        }

        public Result AutoInsertThirdPartyAccount(Entities.Casino.CSCustomerDetail custInfo, CustDetail uplineInfo, int userId, string userName, string ip)
        {
            var customerService = CommonBootStrapper.Locator.GetInstance<ICustomerManager>();
            return customerService.AutoInsertThridPartyAccount(custInfo, uplineInfo, userId, userName, ip);
        }
    }
}
