namespace Fanex.Tarus.Finance.Business.Transfer
{
    using System;

    using DNAide.Data;
    using DNAide.Utils;

    using Fanex.Tarus.Finance.Entities;
    using Fanex.Tarus.Finance.DataAccess.Transfer;
    using Fanex.Tarus.Finance.Model.Transfer;

    using Sunplus.Accounting.Business.BootStrappers;
    using Sunplus.Accounting.Business.Customer;
    using Sunplus.Accounting.Business.Security;
    using Sunplus.Accounting.Core.Common;
    using Sunplus.Accounting.Core.Extensions;
    using System.Collections.Generic;
    using DNAide.Core;
    using System.Data;
    using Sunplus.Accounting.Business;
    using Sunplus.Accounting.Core.Web;

    public class CrossTransferService : ICrossTransferService
    {
        private readonly ICrossTransferAccess _crossTransferAccess;

        public CrossTransferService()
            : this(null)
        {

        }

        public CrossTransferService(ICrossTransferAccess crossTransferAccess)
        {
            _crossTransferAccess = crossTransferAccess ?? new CrossTransferAccess();
        }

        public Result TransferYesterdayTransaction(int accId, string fromCustName, string toCustName, double exRate, double amount, string remark)
        {
            try
            {
                if (accId <= 0 || !Permissions.GetHostPermission("OtherTrans"))
                {
                    return new Result { ErrorCode = 1, ErrorMessage = Fanex.Tarus.Infrastructure.Resource.ErrorLanguage.NotAutherizedFunction };
                }

                var msg = string.Empty;
                var userService = CommonBootStrapper.Locator.GetInstance<IUserService>();
                int fromCustId = userService.GetCustomerIdByUsername(fromCustName);
                bool IsTransfer = Permissions.IsFiTransfer(fromCustId, toCustName, ref msg);
                if (IsTransfer)
                {
                    return _crossTransferAccess.BettransInsertTransfer(accId, fromCustId, toCustName, exRate, amount, remark, Sunplus.Accounting.Core.Helper.Utils.GetClientIP());
                }
                else
                {
                    return new Result { ErrorCode = 1, ErrorMessage = msg };
                }
            }
            catch (AccException ex)
            {
                return new Result { ErrorCode = 1, ErrorMessage = ex.Message };
            }
            catch (Exception ex)
            {

                LogExtensions.ExceptionHandler(ex);
            }
            return new Result { ErrorCode = 1, ErrorMessage = "System Error" };
        }

        public Result CrossCurrencyTransferInsert(FinanceCrossCurrencyTransfer param)
        {
            try
            {
                if (AppSession.AccId <= 0 || !Permissions.GetHostPermission("FinanceTransfer"))
                {
                    return new Result { ErrorCode = 1, ErrorMessage = Fanex.Tarus.Infrastructure.Resource.ErrorLanguage.NotAutherizedFunction };
                }
                Fanex.Tarus.Infrastructure.Caching.CacheHelper.Set<string>("Transfer_ForexAccount_" + AppSession.AccUserName, param.ForexUsername);
                return _crossTransferAccess.CustomerCrossTransferInsert(param.FromCustID, param.ToCustID, param.FromAmount, param.ToAmount, param.FromExRate, param.ToExRate, param.ForexUsername, param.Remark, AppSession.AccId);
            }
            catch (Exception exception)
            {

                LogExtensions.ExceptionHandler(exception);
                return new Result { ErrorCode = 1, ErrorMessage = "System Error" };
            }

        }

        public CustomerExRate SelectCustomerExRate(string username)
        {

            return _crossTransferAccess.SelectCustomerExRate(username);
        }

        public List<ForexAccount> SelectAllForexAccount()
        {
            try
            {
                var listForexAccount = new List<ForexAccount>();
                var dataSet = _crossTransferAccess.SelectAllForexAccount();
                if (dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in dataSet.Tables[0].Rows)
                    {
                        listForexAccount.Add(new ForexAccount
                        {
                            username = CastAide.AsString(row["vvalue"]),
                        });
                    }
                }
                return listForexAccount;
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
                return new List<ForexAccount>();
            }
        }


        public List<ExchangeRate> SelectAllExchangeRate()
        {
            try
            {
                var listEntity = new List<ExchangeRate>();
                var dataSet = _crossTransferAccess.SelectAllExchangeRate();
                if (dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in dataSet.Tables[0].Rows)
                    {
                        listEntity.Add(new ExchangeRate
                        {
                            Code = CastAide.AsString(row["code"]),
                            ActualRate = CastAide.AsDouble(row["actualrate"]),
                            Currency = CastAide.AsString(row["currency"]),
                            ExchangeId = CastAide.AsInt32(row["exchangeid"])
                        });
                    }
                }
                return listEntity;
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
                return new List<ExchangeRate>();
            }
        }
    }
}
