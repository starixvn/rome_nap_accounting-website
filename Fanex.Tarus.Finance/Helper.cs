using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DNAide.Core;

namespace Fanex.Tarus.Finance
{
    using Fanex.Tarus.Finance.DataAccess.Helper;
    using Fanex.Tarus.Infrastructure.Caching;

    using Sunplus.Accounting.Core.Extensions;

    public class Helper
    {
        private  IHelperAccess _helperAccess;

        private const string ForexAccountsKey = "Accounting_Finance_ForexAccounts";

        public DateTime BalanceUpDate { get; set; }
        public DateTime MovebetsDate { get; set; }

        public Helper()
        {
            _helperAccess = new HelperAccess();
            GetBalanceDateInfo();
        }

        public List<string> GetForexAccounts()
        {
            var listForexAccount = new List<string>();
            try
            {

                var objForexAccounts = CacheHelper.Get(ForexAccountsKey);

                if (objForexAccounts != null) return objForexAccounts as List<string>;
                

                var dtFoxexAccount = _helperAccess.GetForexAccounts();
                if (dtFoxexAccount != null && dtFoxexAccount.Rows.Count > 0)
                {
                    listForexAccount = dtFoxexAccount.Rows[0]["vvalue"].ToString().Split(',').ToList();
                    CacheHelper.Set(ForexAccountsKey, listForexAccount, TimeSpan.FromMinutes(10.0));
                }
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }

            return listForexAccount;
            
        }



     

       
       

        private void GetBalanceDateInfo()
        {
            try
            {
                var dtBalanceInfo = _helperAccess.BalanceDateInfo();

                if (dtBalanceInfo != null && dtBalanceInfo.Rows.Count > 0)
                {
                    BalanceUpDate = CastAide.AsDateTime(dtBalanceInfo.Rows[0]["BalanceUpDay"]);
                    MovebetsDate = CastAide.AsDateTime(dtBalanceInfo.Rows[0]["MovebetsDay"]);
                }
            }
            catch (Exception exception)
            {

                LogExtensions.ExceptionHandler(exception);
            }
        }

        public bool CheckIsInternal(string userName)
        {
            return _helperAccess.CheckIsInternal(userName);
        }

        public static List<string> Countries()
        {
            return null;
        }

    }
}
