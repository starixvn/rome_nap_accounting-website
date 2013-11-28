using System;
using System.Collections;
using System.Data;
using DNAide.Data;
using Fanex.Data;
using Sunplus.Accounting.DataAccess.Report;
using Fanex.Tarus.Infrastructure.Extensions;

namespace Sunplus.Accounting.DataAccess.AdoNet.ReportDAO
{
    public class OthersDAO : DataAccess, IReportDAO.IOthersDAO
    {
        public DbResult GetMenuList(string system)
        {
            DbResult result = null;
            try
            {
                Hashtable hashtable = new Hashtable();
                hashtable.Add("custid", Sunplus.Accounting.Core.Common.AppSession.AccId);
                hashtable.Add("system", system);

                using (ObjectDb db = new ObjectDb("Acc_MenuSelByCustID", 600))
                {
                    db.AssignParamValues(hashtable);

                    var ds = db.ExecuteDataSet();
                    var output = db.GetOuputParams();

                    result = new DbResult(output, ds);
                }
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }
            return result;
        }

        public DbResult GetMixParlayWonList(bool isSoccer, DateTime date)
        {
            DbResult result = null;
            try
            {
                Hashtable hashtable = new Hashtable();
                hashtable.Add("issoccer", isSoccer);
                hashtable.Add("winlostdate", date);
                hashtable.Add("movebetsday", ReportUtils.BalanceUp.MoveBetsDay);
                hashtable.Add("balanceupday", ReportUtils.BalanceUp.BalanceUpDay);

                using (ObjectDb db = new ObjectDb("MixparlayWon", 600))
                {
                    db.AssignParamValues(hashtable);

                    var ds = db.ExecuteDataSet();
                    var output = db.GetOuputParams();

                    result = new DbResult(output, ds);
                }
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }
            return result;
        }

        public DbResult CombinationGetMixParlayWonList(bool isSoccer, DateTime date)
        {
            DbResult result = null;
            try
            {
                
                Hashtable hashtable = new Hashtable();
                hashtable.Add("issoccer", isSoccer);
                hashtable.Add("winlostdate", date);
                hashtable.Add("movebetsday", ReportUtils.BalanceUp.MoveBetsDay);
                hashtable.Add("balanceupday", ReportUtils.BalanceUp.BalanceUpDay);
                hashtable.Add("bettype", 29);

                using (ObjectDb db = new ObjectDb("MixparlayWon", 600))
                {
                    db.AssignParamValues(hashtable);

                    var ds = db.ExecuteDataSet();
                    var output = db.GetOuputParams();

                    result = new DbResult(output, ds);
                }
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }
            return result;
        }

        public DataSet GetCheckBet(DateTime winlostDate)
        {
            using (ObjectDb objDb = new ObjectDb("Acc_checkhack2", 180))
            {
                var p = new Hashtable();
                p.Add("winlostdate", winlostDate);
                objDb.AssignParamValues(p);

                return objDb.ExecuteDataSet();
            }
        }
    }
}