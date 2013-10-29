
namespace Fanex.Tarus.Reports.DataAccess.NumberGameWinLoss
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Data;
    using Fanex.Data;
    using Fanex.Tarus.Reports.Entities.NumberGameWinLoss;
    using Sunplus.Accounting.Core.Common;
    using Sunplus.Accounting.Core.Extensions;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class NumberGameWinLossAccess : INumberGameWinLossAccess
    {
        public DataTable GetCurrencyList()
        {
            DataTable dt = new DataTable();
            try
            {
                using (ObjectDb db = new ObjectDb("Acc_ExchangeSel"))
                {
                    db.AddInParameter("system", DbType.String, "ACCOUNTING");
                    var result = db.ExecuteDataSet();
                    if (result.Tables.Count > 0)
                    {
                        dt = result.Tables[0];
                    }
                }
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }
            return dt;
        }

        public DataTable GetBettypeList()
        {

            DataTable dt = new DataTable();
            try
            {
                using (ObjectDb db = new ObjectDb("Acc_BetTypeSel_NumberGame"))
                {
                    var result = db.ExecuteDataSet();
                    if (result.Tables.Count > 0)
                    {
                        dt = result.Tables[0];
                    }
                }
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }
            return dt;
        }

        public DataTable GetLicensees()
        {

            DataTable dt = new DataTable();
            try
            {
                using (ObjectDb db = new ObjectDb("Acc_Rpt_NumberGame_LicenseesSel"))
                {
                    var result = db.ExecuteDataSet();
                    if (result.Tables.Count > 0)
                    {
                        dt = result.Tables[0];
                    }
                }
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }
            return dt;
        }
    }
}
