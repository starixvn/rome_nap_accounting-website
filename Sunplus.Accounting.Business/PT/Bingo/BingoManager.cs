using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ServiceModel.Activation;
using System.Threading;
using System.Web;
using Microsoft.Practices.Unity;
using Sunplus.Accounting.Business.Security;
using Sunplus.Accounting.Core.Common;
using Sunplus.Accounting.Core.Configurations;
using Sunplus.Accounting.Core.Extensions;
using Sunplus.Accounting.Entities.Casino;
using Sunplus.Bingo.Core;
using Sunplus.Bingo.Entities;
using Sunplus.Bingo.Services;
using Result = Sunplus.Bingo.Entities.Result;

/// <summary>
/// Bingo Controller
/// </summary>
namespace Sunplus.Accounting.Business.PT.Bingo
{
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class BingoManager:BaseManager, IBingoService
    {
        public Sunplus.Bingo.Entities.Customer GetCustomerSetting(int custID, int updatedByRoleID)
        {
            var customer = new Sunplus.Bingo.Entities.Customer();
            try
            {
                var customerService = BingoContainer.Current.Resolve<ICustomerService>();

                IResult result = new Result();

                customer = customerService.GetCustomerSetting(custID, updatedByRoleID, ref result) as Sunplus.Bingo.Entities.Customer;

                if (customer != null)
                {
                    customer.Result = result as Result;
                }
            }
            catch (Exception ex)
            {
                customer.Result = new Result(1, ex.Message);
            }
            
            return customer;
            
        }

        public Result UpdateCustomerSetting(Sunplus.Bingo.Entities.Customer customer, int updatedByRoleID)
        {
            //throw new NotImplementedException();
            IResult result;
            try
            {
                if (!Permissions.GetHostPermission(Enumerations.Functions.Customer.OtherCust))
                    return new Result(Sunplus.Accounting.Core.Common.Constants.Error.NotAutherized, ResourceExtension.GetResourceValue(ResourceExtension.Resources.ErrorMsg, "_500"));
                var customerService = BingoContainer.Current.Resolve<ICustomerService>();
                string fromIP = HttpContext.Current.Request.UserHostAddress;
                int modifyBy = AppSession.AccId;
                string modifyUserName = AppSession.AccUserName;
                result =
                    customerService.UpdateCustomerSetting(customer, updatedByRoleID, fromIP, modifyBy, modifyUserName, 3) as
                    Result;
            }
            catch (Exception ex)
            {
                
                result = new Result(1, ex.Message);
            }
            return (result as Result);
        }

        public Result ResetMaxTransfer(int custId)
        {
            if (!Permissions.GetHostPermission(Enumerations.Functions.Customer.Customer))
                return new Result(Sunplus.Accounting.Core.Common.Constants.Error.NotAutherized, ResourceExtension.GetResourceValue(ResourceExtension.Resources.ErrorMsg, "_500"));
            var customerService = BingoContainer.Current.Resolve<ICustomerService>();
            string fromIP = HttpContext.Current.Request.UserHostAddress;
            int modifyBy = AppSession.AccId;
            string modifyUserName = AppSession.AccUserName;
            var result =
                customerService.ResetMaxTransfer(custId, fromIP, modifyBy, modifyUserName, 3) as
                Result;
            return result;
        }

        public IResult UpdateExchangeRate(string siteNo, int exchangeID, string currencyName, double actualRate, int modifyBy, string modifyByUsename, string fromIp)
        {
            var customerService = BingoContainer.Current.Resolve<ICustomerService>();
            return customerService.UpdateExchangeRate(siteNo, exchangeID, currencyName, actualRate, modifyBy, modifyByUsename,
                                               fromIp);
        }

        public string ResourcesExtension { get; set; }

        #region Auto insert Bingo account - background worker
        private static BackgroundWorker _worker = null;
        private static Queue<CSCustomerDetail> _queueCsCustomer = new Queue<CSCustomerDetail>();
        private static Queue<int> _queueUserId = new Queue<int>();
        private static Queue<string> _queueUserName = new Queue<string>();
        private static Queue<string> _queueIp = new Queue<string>();
        private static Queue<bool > _queueEnableBingo = new Queue<bool>();
        public void AutoInsertBingoAccount(CSCustomerDetail customerInfo, int userId, string userName, string ip, bool enabledBingo)
        {
            try
            {
                #region assign data
                _queueCsCustomer.Enqueue(customerInfo);
                _queueUserId.Enqueue(userId);
                _queueUserName.Enqueue(userName);
                _queueIp.Enqueue(ip);
                _queueEnableBingo.Enqueue(enabledBingo);
                #endregion
                using (_worker = new BackgroundWorker())
                {
                    _worker.WorkerSupportsCancellation = true;
                    _worker.DoWork += new DoWorkEventHandler(WorkerRunning);
                    _worker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(WorkerCompleted);
                    if(!_worker.IsBusy)
                        _worker.RunWorkerAsync();
                }
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }
        }

        #region background worker
        /**
         * Function: Backgroundworker running
         */
        private static void WorkerRunning(object sender, DoWorkEventArgs e)
        {
            if (_queueCsCustomer.Count > 0)
            {
                Thread.Sleep(100);
                InsertNewCustomerAccount();
            }
            else
            {
                _worker.CancelAsync();
            }
        }
        /**
         * Function: Backgroundworker commpleted
         */
        private static void WorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            //_worker.Dispose();
            //TODO
        }
        #endregion

        #region priavate methods
        private static void InsertNewCustomerAccount()
        {
            try
            {
                var enableBingo = _queueEnableBingo.Dequeue();
                if (enableBingo)
                {
                    var bingoLibray = new LiveCasinoLibrary.SBAgent.Customer();
                    var custInfo = _queueCsCustomer.Dequeue();
                    var ip = _queueIp.Dequeue();
                    var userId = _queueUserId.Dequeue();
                    var userName = _queueUserName.Dequeue();
                    //Thread.Sleep(6000);
                    //var recalIndex = 0;
                    var isSuccess = true;

                    isSuccess = bingoLibray.AutoInsertAccount(custInfo.CasinoNo, custInfo.IBCCustID, custInfo.UserName,
                                                              custInfo.SIBCCustID, custInfo.MIBCCustID,
                                                              custInfo.AIBCCustID, custInfo.RoleId.ToString(),
                                                              custInfo.ExchangeID, DateTime.Now, ip, userId, userName, 3);

                    Thread.Sleep(2000);
                    if (isSuccess)
                    {
                        Sunplus.Bingo.Entities.IResult iResult = null;
                        var bingoDao = BingoContainer.Current.Resolve<Sunplus.Bingo.Data.ICustomerDAO>();
                        bingoDao.UpdateSyncBingo(custInfo.IBCCustID, true, ip, userId, ref iResult);
                    }
                }

            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }
        }

        #endregion
        #endregion
    }
}
