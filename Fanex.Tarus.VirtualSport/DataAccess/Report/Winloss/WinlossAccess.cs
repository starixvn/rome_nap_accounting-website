// -----------------------------------------------------------------------
// <copyright file="WinlossVSAccess.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Tarus.VirtualSport.DataAccess.Report.Winloss
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Data;
    using Fanex.Data;
    using DNAide.Core;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class WinlossAccess: IWinlossAccess
    {

        public DataSet GetDailyWinloss(DateTime fromDate, DateTime toDate, bool isExclude, int DrawFilter, string sportType)
        {
            DataSet ds = null;
            using (ObjectDb objDB = new ObjectDb("DailyWinlossVirtualSport"))
            {
                objDB.AddInParameter("fromdate", DbType.DateTime, fromDate);
                objDB.AddInParameter("todate", DbType.DateTime, toDate);
                objDB.AddInParameter("excludetest", DbType.Boolean, isExclude);
                objDB.AddInParameter("drawfilter", DbType.Int32, DrawFilter);
                objDB.AddInParameter("sporttype", DbType.String, sportType);
                ds = objDB.ExecuteDataSet();
                
            }
            return ds;
        }

        public DataTable GetSportType()
        {
            DataTable dtb = null;
            using (ObjectDb objDb = new ObjectDb("GetSportType"))
            {
                var ds = objDb.ExecuteDataSet();
                if (ds != null)
                {
                    dtb = ds.Tables[0];
                }
            }
            return dtb;
        }




        public DataTable GetDailyWinlossByCurrency(DateTime date, string sportType, bool isExclude, int DrawFilter, string currency)
        {
            DataTable dtb = null;
            using (ObjectDb objDB = new ObjectDb("WinlossVirtualSportByCurrency"))
            {
                objDB.AddInParameter("date", DbType.DateTime, date);
                objDB.AddInParameter("excludetest", DbType.Boolean, isExclude);
                objDB.AddInParameter("drawfilter", DbType.Int32, DrawFilter);
                objDB.AddInParameter("sporttype", DbType.String, sportType);
                objDB.AddInParameter("currency", DbType.String, currency);
                var ds = objDB.ExecuteDataSet();
                if(ds != null)
                    dtb = ds.Tables[0];
            }
            return dtb;
        }
    }
}
